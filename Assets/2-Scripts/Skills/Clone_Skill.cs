using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Stats")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetUpClone(clonePosition, cloneDuration, canAttack);
    }

    /*protected override void CheckUnlock()
    {
        base.CheckUnlock();
        UnlockCloneAttack();
        UnlockAggresiveClone();
        UnlockMultiColne();
        UnlockCrystalInstead();
    }*/
}
