using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public bool isBusy { get; private set; }
    public bool isSpeaking;

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public bool canDoubleJump;
    public float swordreturnImpact;
    [HideInInspector] public float defaultMoveSpeed;
    private float defaultJumpSpeed;

    [Header("Particulas info")]
    [SerializeField] public ParticleSystem polvoPies;
    [SerializeField] public ParticleSystem polvoSalto;
    [HideInInspector] public ParticleSystem.EmissionModule emisionPolvoPies;
    [HideInInspector] public bool cayendo = false;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    [Header("Camera info")]
    private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;
    [HideInInspector] public float fallSpeedYDampingChangeThresold;

    public UnityEngine.UI.Image healthBar;

    [Header("Estamina info")]
    public Stat maxStamina;
    public float currentStamina;
    public UnityEngine.UI.Image staminaBar;
    public float staminaRegen = 5;
    [SerializeField] private Image poti;
    [SerializeField] private Text potiText;
    public bool isHealing;

    [Header("Mana info")]
    public Stat maxMana;
    public float currentMana;
    public UnityEngine.UI.Image manaBar;
    public float manaRegen = 10;


    #region Angularidad
    [Header("Angularidad")]
    public float anguloMax;
    [HideInInspector] public Vector2 capsuleSize;
    [HideInInspector] public Vector2 posPies;
    [HideInInspector] public bool enPendiente;
    [HideInInspector] public float anguloLateral;
    [HideInInspector] public float anguloPendiente;
    [HideInInspector] public float anguloAnterior;
    [HideInInspector] public Vector2 anguloPer;
    [HideInInspector] public bool puedeCaminar;
    public PhysicsMaterial2D sinFriccion;
    public PhysicsMaterial2D maxFriccion;
    #endregion
    [Header("Deslizamiento en pendiente")]
    [SerializeField] private bool wasPoisoned = false;
    [SerializeField] private float wateredTimer = 1;

    public float dashDir { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideSate wallSlideSate { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchState catchSword { get; private set; }
    public PlayerDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        wallSlideSate = new PlayerWallSlideSate(this, stateMachine, "WallSlide");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchState(this, stateMachine, "CatchSword");
        defaultMoveSpeed = moveSpeed;
        deadState = new PlayerDeadState(this, stateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultJumpSpeed = jumpForce;
        defaultDashSpeed = dashSpeed;
        capsuleSize = capsule.size;
        emisionPolvoPies = polvoPies.emission;

        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
        fallSpeedYDampingChangeThresold = CameraManager.instance.fallSpeedYDampingChangeTheshold;

        Color color = skill.sword.normalImage.color;
        color.a = 1.0f;
        skill.sword.normalImage.color = color;

        this.currentStamina = maxStamina.GetValue();
        //UpdateStaminaUI();
        InvokeRepeating("RegenerateStamina", 1f, 1f);// esto no me gusta del todo
                                                     
        this.currentMana = maxMana.GetValue();
        InvokeRepeating("RegenMana", 1f, 1f);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        CheckForDashInput();
        wateredTimer -= Time.deltaTime;

        if (wateredTimer < 0)
        {
            wasPoisoned = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Inventory.instance.UseFlask();
            StartCoroutine(BanishPoti());
            
        }

        CheckWatered();
        UpdateHealthUI();
        UpdateStaminasUI();
    }
    IEnumerator BanishPoti()
    {
        Color color = poti.color;
        color.a = 0.1f;
        poti.color = color;
        float startColor = 1.0f;
        float finalColor = 0.0f;

        Color textColor = potiText.color;
        textColor.a = 0.01f;
        potiText.color = textColor;

        //alfa del texto es 0%
        while (finalColor < 12f)
        {
            finalColor += Time.deltaTime;
            color.a = Mathf.Lerp(0.1f, startColor, finalColor / 12f);
            poti.color = color;
            yield return null; 
        }

        color.a = startColor;
        poti.color = color;
        //alfa del texto es 100%

        textColor.a = 1.0f;
        potiText.color = textColor;
    }
    public IEnumerator TakePoti()
    {
        isHealing = true;
        yield return new WaitForSeconds(1f);
        isHealing = false;
    }

    public override void Flip()
    {
        base.Flip();

        cameraFollowObject.CallTurn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isWatered = true;
            rb.drag = 100;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isWatered = false;
            rb.drag = 0;
        }
    }

    public void CheckWatered()
    {
        if (isWatered)
        {
            Swiming();
            if (!wasPoisoned)
            {
                wateredTimer = 1f;

                gameObject.GetComponent<CharacterStats>().TakeDamage(GetComponent<CharacterStats>().maxHealth.GetValue() * .05f);
                //fx.IgniteFxFor(ailmentsDuration);
                wasPoisoned = true;
            }
        }
        else
        {
            ReturnDefaultSpeed();
        }
    }
    private void Swiming()
    {
        if (!IsGroundDetected())
        {
            rb.velocity = new Vector3(rb.velocity.x / 2, -10, 0);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x / 2, 0, 0);
        }
        jumpForce = jumpForce / 2;
    }
    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        moveSpeed = moveSpeed * (1 - slowPercentage);
        jumpForce = jumpForce * (1 - slowPercentage);
        dashSpeed = dashSpeed * (1 - slowPercentage);
        anim.speed = anim.speed * (1 - slowPercentage);

        Invoke("ReturnDefaultSpeed", slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpSpeed;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(seconds);

        isBusy = false;
    }

    public void AnimacionTriiger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected() || bossSpawning || isSpeaking || this.currentStamina < 40 || PauseMenu.instance.isPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() && !isWatered)
        {
            this.currentStamina -= 40;             

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            stateMachine.ChangeState(dashState);
            UpdateStaminasUI();
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    private void UpdateHealthUI()
    {
        float currentHealth = GetComponent<CharacterStats>().currentHealth;
        float maxHealth = GetComponent<CharacterStats>().maxHealth.GetValue();

        healthBar.fillAmount = currentHealth / maxHealth;
    }
    public void UpdateStaminasUI()
    {
        float currentStamina = this.currentStamina;
        float maxStamina = this.maxStamina.GetValue();

        staminaBar.fillAmount = currentStamina / maxStamina;
    }

    public void UpdateManaUI()
    {
        float currentMana = this.currentMana;
        float maxMana = this.maxMana.GetValue();

        manaBar.fillAmount = currentMana / maxMana;
    }

    public void Speak()
    {
        rb.velocity = new Vector2(0, 0);
        isSpeaking = true;
    }

    public void StopSpeak()
    {
        isSpeaking = false;
    }

    public override void DamageImpact()
    {
        base.DamageImpact();
        isSpeaking = false;
    }

    void RegenerateStamina()
    {
        this.currentStamina += staminaRegen;
        this.currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina.GetValue());
        UpdateStaminasUI();
    }

    void RegenMana()
    {
        if (!sword) 
        {
            this.currentMana += manaRegen;   
        }
        this.currentMana = Mathf.Clamp(currentMana, 0f, maxMana.GetValue());
        UpdateManaUI();
    }

    public void CutSceneTransparente()
    {
        //Debug.Log("Aqui estoy, debiendo de ser transparente.");
        fx.MakeTransprent(true);
        Speak();
    }

    public void CutSceneVisible()
    {
        fx.MakeTransprent(false);        
    }

    public void BossSpawning()
    {
        bossSpawning = true;
    }

    public void BossSpawned()
    {
        bossSpawning = false;
    }

    public void MandarDatos()
    {
        if (PlayerPrefs.GetString("Logged").Equals("S"))
        {
            int score = 0;

            /*if (PlayerManager.instance.bossKilled)
                score += 1000;*/

            score += PlayerManager.instance.currency;

            if (GetComponent<PlayerStats>().currentHealth > 0)
                score += (int)GetComponent<PlayerStats>().maxHealth.GetValue() / (int)GetComponent<PlayerStats>().currentHealth * 1000;
            else
                score += 0;

            PlayFabManager.instance.SendLeaderBoard(score);
        }
    }

    public void StartMal()
    {
        Start();
    }
}
