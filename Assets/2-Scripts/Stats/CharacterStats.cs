using System.Collections;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;           // 1 punto == 1 en damage y 1% de crit.power
    public Stat agulity;            // 1 punto == 1% evasion y 1% de crit.chance
    public Stat inteligence;        // 1 punto == 3 de magic damage y 3 de magic resistance
    public Stat vitality;           // 1 punto == aumento de vida en [3-5]

    [Header("Ofensive stats")]
    public Stat damage;
    public Stat critChance;         
    public Stat critPower;          // Default 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat magicResistance;
    public Stat evasion;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    [Header("Status info")]
    public bool isIgnited;          // Daño por tiempo
    public bool isChilled;          // Reduce la armadura y resistencia magica en 25%
    public bool isShocked;          // Reduce el precisión em 25% 

    [Header("Damge Over Time info")]
    [SerializeField] private float ailmentsDuration = 3;
    private float igniteTimer;
    private float chillTimer;
    private float shockTimer;

    [Header("Ignite info")]
    private float igniteDamageCooldown = .3f;
    private float igniteDamageTimer;
    private float igniteDamage;

    [Header("Shock info")]
    [SerializeField] private GameObject shockStrikePrefab;
    private float shockDamage;

    [Header("Character info")]
    public string nameChar;

    public float currentHealth;

    public System.Action onHealthChanged;
    protected bool isDead;
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
        //damage.AddModifier(5.5f);
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;

        if (igniteTimer < 0)
        {
            isIgnited = false;
        }

        if (chillTimer < 0)
        {
            isChilled = false;
        }

        if (shockTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));

    private IEnumerator VulnerableCorutine(float _duartion)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duartion);

        isVulnerable = false;
    }

    public virtual void DoDamage(CharacterStats character)
    {
        if (character.isInvincible) 
        {
            return;
        }           

        //Comprobar si esquiva el ataque
        if (TargetCanAvoidAttack(character))
        {
            return;
        }

        //Calcular el daño recibido
        float totalDMG = damage.GetValue() + strength.GetValue();

        //Calcular critico
        if (CanCrit())
        {
            totalDMG = CalculateCriticalDamage(totalDMG);
        }

        //Reducirlo por la armadura
        totalDMG = CheckTargetArmor(character, totalDMG);

        //Aplicar el daño
        character.TakeDamage(totalDMG);  
        //DoMagicalDamage(character);
    }

    #region Magical damage an ailments
    public virtual void DoMagicalDamage(CharacterStats character)
    {
        //Calculamos el daño elemental
        float fDamage = fireDamage.GetValue();          //Fuego
        float iDamage = iceDamage.GetValue();           //Hielo
        float lDamage = lightningDamage.GetValue();     //Relampago

        //Calculamos el daño magico raw
        float totalMagicalDamage = fDamage + iDamage + lDamage + inteligence.GetValue();

        //Aplicamos resistencias
        totalMagicalDamage = CheckTargetMagicalResistance(character, totalMagicalDamage);

        //Aplicamos el daño.
        character.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fDamage, iDamage, lDamage) <= 0)
        {
            return;
        }

        AttempToApplyAilments(character, fDamage, iDamage, lDamage);
    }

    private void AttempToApplyAilments(CharacterStats character, float fDamage, float iDamage, float lDamage)
    {
        //Crear estados alterados
        //Comprobamos el daño predominante
        bool canApplyIgnite = fDamage > iDamage && fDamage > lDamage;
        bool canApplyChill = iDamage > fDamage && iDamage > lDamage;
        bool canApplyShock = lDamage > fDamage && lDamage > iDamage;

        while (!canApplyIgnite || !canApplyChill || !canApplyShock)
        {
            if (Random.value < .5f && fDamage > 0)
            {
                canApplyIgnite = true;
                character.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && iDamage > 0)
            {
                canApplyChill = true;
                character.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && lDamage > 0)
            {
                canApplyShock = true;
                character.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            character.SetupIgniteDamage(fDamage * .2f);
        }

        if (canApplyShock)
        {
            character.SetUpShockDamage(lDamage * .2f);
        }
        //Aplicamos el estado
        character.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (ignite && canApplyIgnite)
        {
            isIgnited = ignite;
            igniteTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (chill && canApplyChill)
        {
            isChilled = chill;
            chillTimer = ailmentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWirhShockStrike();
            }
        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);
            if (currentHealth < 0 && !isDead)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public void ApplyShock(bool shock)
    {
        if (isShocked)
        {
            return;
        }

        isShocked = shock;
        shockTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWirhShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemigo>() != null && Vector2.Distance(transform.position, hit.transform.position) > 4)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(float dmg)
    {
        igniteDamage = dmg;
    }

    public void SetUpShockDamage(float dmg)
    {
        shockDamage = dmg;
    }
    #endregion

    public virtual void TakeDamage(float dmg)
    {
        DecreaseHealthBy(dmg);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    protected virtual void DecreaseHealthBy(float dmg)
    {
        currentHealth -= dmg;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public void MakeInvincible(bool invincible)
    {
        isInvincible = invincible;
    }

    #region Calculations
    private float CheckTargetArmor(CharacterStats character, float totalDMG)
    {
        if (character.isChilled)
        {
            totalDMG -= character.armor.GetValue() * .75f;
        }
        else
        {
            totalDMG -= character.armor.GetValue();
        }
        
        totalDMG = Mathf.Clamp(totalDMG, 0, int.MaxValue);
        return totalDMG;
    }

    private float CheckTargetMagicalResistance(CharacterStats character, float totalMagicalDamage)
    {
        //Reducimos la defensa magica
        if (character.isChilled)
        {
            totalMagicalDamage -= (character.magicResistance.GetValue() * .75f) + (character.inteligence.GetValue() * 3);
        }
        else
        {
            totalMagicalDamage -= character.magicResistance.GetValue() + (character.inteligence.GetValue() * 3);
        }        

        //Evitamos los valores negativos
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats character)
    {
        float totalEvasion = character.evasion.GetValue() + character.agulity.GetValue();

        if (isShocked)
        {
            totalEvasion += 25;
        }

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;

        }

        return false;
    }

    private bool CanCrit()
    {
        float totalCriticalChance = critChance.GetValue() + agulity.GetValue();

        if (UnityEngine.Random.Range(0, 100) <= totalCriticalChance)
        {
            return true; 
        }

        return false;
    }

    private float CalculateCriticalDamage(float damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = damage * totalCritPower;

        return critDamage;
    }

    public float GetMaxHealthValue()
    {
        return maxHealth.GetValue() + (vitality.GetValue() * 5); 
    }
    #endregion
}
