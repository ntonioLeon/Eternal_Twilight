using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effects/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecutedEffect(Transform enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        float healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //Debug.Log(healAmount);

        playerStats.IncreaseHealthBy(healAmount);

        playerStats.currentHealth += healAmount;
    }
}
