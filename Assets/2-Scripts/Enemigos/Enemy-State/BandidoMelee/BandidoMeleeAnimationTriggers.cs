using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandidoMeleeAnimationTriggers : MonoBehaviour
{
    private BandidoMelee enemy => GetComponentInParent<BandidoMelee>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
