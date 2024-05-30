using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fire strike effect", menuName = "Data/Item Effects/Fire Strike")]
public class FireEffect : ItemEffect
{
    [SerializeField] private GameObject fireStrikeEffectPrefab;

    public override void ExecutedEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(fireStrikeEffectPrefab, enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
