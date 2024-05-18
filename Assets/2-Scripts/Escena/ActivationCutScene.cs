using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActivationCutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector playableDirector;
    
    public void LanzarCutscene()
    {
        playableDirector.Play();
    }
}
