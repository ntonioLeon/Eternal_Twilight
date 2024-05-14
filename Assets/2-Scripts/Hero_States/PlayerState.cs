using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() 
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        if (PauseMenu.instance.isPaused)
        {
            xInput = 0;
            yInput = 0;
            player.SetZeroVelocity();
            return;
        }

        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        CheckPendiente();
        GoingDown();
        CheckPolvoPies();
        CheckPolvoSalto();
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckPolvoSalto()
    {
        if (rb.velocity.y < 0 && player.IsGroundDetected() && player.cayendo)
        {
            player.polvoSalto.Play();
            player.cayendo = false;
        }
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    private void CheckPolvoPies()
    {
        if (player.IsGroundDetected() && xInput != 0)
        {
            player.emisionPolvoPies.rateOverTime = 35;
        }
        else
        {
            player.emisionPolvoPies.rateOverTime = 0;
        }
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void CheckPendiente()
    {
        // COLISION HORIZONTAL.
        player.posPies = player.transform.position - (Vector3)(new Vector2(0.0f, player.capsuleSize.y / 2 + .2f));
        RaycastHit2D hitForward = Physics2D.Raycast(player.posPies, Vector2.right, player.pendienteCheckDistance, player.whatIsGround);
        RaycastHit2D hitBackward = Physics2D.Raycast(player.posPies, -Vector2.right, player.pendienteCheckDistance, player.whatIsGround);
        //Debug.DrawRay(player.posPies, Vector2.right * player.pendienteCheckDistance, Color.red);
        //Debug.DrawRay(player.posPies, -Vector2.right * player.pendienteCheckDistance, Color.red);
        if (hitForward)
        {
            player.enPendiente = true;
            player.anguloLateral = Vector2.Angle(hitForward.normal, Vector2.up);
        }
        else if (hitBackward)
        {
            player.enPendiente = true;
            player.anguloLateral = Vector2.Angle(hitBackward.normal, Vector2.up);
        }
        else
        {
            player.enPendiente = false;
            player.anguloLateral = 0f;
        }
        // COLISIONES VERTICALES.
        RaycastHit2D hitDownward = Physics2D.Raycast(player.posPies, Vector2.down, player.pendienteCheckDistance, player.whatIsGround);
        if (hitDownward)
        {
            player.anguloPendiente = Vector2.Angle(hitDownward.normal, Vector2.up);
            player.anguloPer = Vector2.Perpendicular(hitDownward.normal).normalized;
            if (player.anguloPendiente != player.anguloAnterior)
            {
                player.enPendiente = true;
            }
            player.anguloAnterior = player.anguloPendiente;
            //Debug.DrawRay(hitDownward.point, player.anguloPer, Color.blue);
            //Debug.DrawRay(hitDownward.point, hitDownward.normal, Color.green);
        }
        // ANGULO MAXIMO
        if (player.anguloPendiente > player.anguloMax || player.anguloLateral > player.anguloMax)
        {
            player.puedeCaminar = false;
        }
        else
        {
            player.puedeCaminar = true;
        }

        if (player.enPendiente && player.puedeCaminar && xInput == 0)
        {
            rb.sharedMaterial = player.maxFriccion;
        }
        else
        {
            rb.sharedMaterial = player.sinFriccion;
        }
    }
    private void GoingDown()
    {
        if (Input.GetAxisRaw("Vertical")==-1 && player.IsGroundDetected()) // 
        {
            RaycastHit2D hitGround = Physics2D.Raycast(player.transform.position, Vector2.down, (player.GetComponent<CapsuleCollider2D>().size.y) + .25f, player.whatIsGround);
            Debug.DrawRay(player.transform.position, Vector2.down * ((player.GetComponent<CapsuleCollider2D>().size.y) + .25f), Color.cyan);
            if (hitGround)
            {
                if (hitGround.transform.gameObject.tag == "SpecialGround")
                {
                    //bool a = hitGround.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger;
                    //string b = hitGround.transform.gameObject.tag;
                    //string c = hitGround.transform.gameObject.layer.ToString();
                    //BoxCollider2D platform= hitGround.transform.gameObject.GetComponent<BoxCollider2D>();
                    player.StartCoroutine(CorroutineGoingDown());
                    //hitGround.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    //hitGround.transform.gameObject.tag = "Untagged";
                    //hitGround.transform.gameObject.layer = LayerMask.NameToLayer("Default");
                    //Debug.Log("Para Abajo");
                    //Debug.Log("Trigger: " + a + "---" + hitGround.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger);
                    //Debug.Log("Tag: " + b + "---" + hitGround.transform.gameObject.tag);
                    //Debug.Log("Layer: " + c + "---" + hitGround.transform.gameObject.layer.ToString());
                }
            }
        }
    }

    private IEnumerator CorroutineGoingDown()
    {
        //platform.isTrigger = true;

        player.GetComponent<CapsuleCollider2D>().isTrigger= true;

        yield return new WaitForSeconds(.18f);

        player.GetComponent<CapsuleCollider2D>().isTrigger = false;
        //platform.isTrigger = false;
    }
}
