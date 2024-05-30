using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item Effects/Thunder Strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikeEffectPrefab;

    public override void ExecutedEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikeEffectPrefab, enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
