using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy_NightBorn : Enemigo
{
    #region
    public NightBornBattleState battleState { get; private set; }
    public NightBornAttackState attackState { get; private set; }
    public NightBornIdleState idleState { get; private set; }
    public NightBornDeadState deadState { get; private set; }
    public NightBornTeleportState teleportState { get; private set; }
    public NightBornSpellCastState spellCastState { get; private set; }
    #endregion

    public bool bossFightBegun;
    [SerializeField] private GameObject deathPrefab;

    [Header("SpellCast details")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown;
    [SerializeField] private Vector2 spellOffset;

    [Header("Teleport detaills")]
    [SerializeField] private BoxCollider2D battleArea;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaulChanceToTeleport = 15;

    public GameObject healthBar;
    private Color originColor;

    protected override void Awake()
    {
        base.Awake();

        idleState = new NightBornIdleState(this, stateMachine, "Idle", this);

        battleState = new NightBornBattleState(this, stateMachine, "Move", this);
        attackState = new NightBornAttackState(this, stateMachine, "Attack", this);

        deadState = new NightBornDeadState(this, stateMachine, "Idle", this);
        teleportState = new NightBornTeleportState(this, stateMachine, "Teleport", this);
        spellCastState = new NightBornSpellCastState(this, stateMachine, "SpellCast", this);
    }

    protected override void Start()
    {
        base.Start();
        /*
        originColor = healthBar.color;        
        */
        //healthBar.color.a

        AudioManager.instance.PlaySFX(22);
        stateMachine.Initioalize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        UpdateHealthUI();
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public override void Die()
    {
        base.Die();
        UI_Boss.instance.BossDeactivation();
        AudioManager.instance.PlaySFX(16);
        stateMachine.ChangeState(deadState);
    }

    public void Morir()
    {
        Instantiate(deathPrefab, transform.position, Quaternion.identity);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;


        float xOffset = 0;

        if (player.rb.velocity.x != 0)
            xOffset = player.facingDir * spellOffset.x;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + spellOffset.y);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<NightBornSpell_Controller>().SetupSpell(stats);
    }

    public void FindPosition()
    {
        float xPos = Random.Range(battleArea.bounds.min.x + 3, battleArea.bounds.max.x - 3);
        float yPos = Random.Range(battleArea.bounds.min.y + 3, battleArea.bounds.max.y - 3);

        transform.position = new Vector3(xPos, yPos);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBellow().distance + (capsule.size.y / 2));

        if (!GroundBellow() || SomethingIsArround())
        {
            Debug.Log("Looking for new positions");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBellow()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    }

    private bool SomethingIsArround()
    {
        return Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBellow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            amountOfSpells -= 1;
            chanceToTeleport = defaulChanceToTeleport;
            return true;
        }

        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCooldown)
        {
            return true;
        }

        return false;
    }

    private void UpdateHealthUI()
    {
        if (player.GetComponent<PlayerStats>().isDead)
        {
            healthBar.SetActive(false);
            StartCoroutine(DespawnBoss());
            return;
        }
        else
        {
            healthBar.SetActive(true);
        }

        float currentHealth = GetComponent<CharacterStats>().currentHealth;
        float maxHealth = GetComponent<CharacterStats>().maxHealth.GetValue();

        healthBar.GetComponentInChildren<UnityEngine.UI.Image>().fillAmount = currentHealth / maxHealth;
    }

    IEnumerator DespawnBoss()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Find("Boss Panel").SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void StartBattle()
    {
        bossFightBegun = true;
    }
}
