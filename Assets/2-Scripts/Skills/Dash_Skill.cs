using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Cloness");
    }

    /*protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockClonmeOnArrival();
    }*/
}
