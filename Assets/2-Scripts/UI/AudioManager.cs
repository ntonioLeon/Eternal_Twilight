using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    

    public AudioSource[] soundEffects;

    public AudioSource[] soundMusic;

    [SerializeField]
    public AudioMixer generalMixer, musicMixer, effectsMixer;

    [Range(-80, 10)]
    public float generalVol;
    [Range(-80, 10)]
    public float musicVol;
    [Range(-80, 10)]
    public float effectsVol;

    public Slider generalSldr, musicSldr, effectsSldr;

    private int currentSound;
    private float currentVol;
    public bool muted=false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        generalSldr.value = generalVol;
        generalSldr.minValue = -80;
        generalSldr.maxValue = 10;
        
        musicSldr.value = musicVol;
        musicSldr.minValue = -80;
        musicSldr.maxValue = 10;

        effectsSldr.value = effectsVol;
        effectsSldr.minValue = -80;
        effectsSldr.maxValue = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        GeneralVolume();
        MusicVolume();
        if (!muted) 
        {
            EffectVolume();
        }
        
    }

    public void GeneralVolume()
    {
        generalMixer.SetFloat("General", generalSldr.value);
    }

    public void MusicVolume()
    {
        generalMixer.SetFloat("Music", musicSldr.value -25f);
    }

    public void EffectVolume()
    {
        generalMixer.SetFloat("Effects", effectsSldr.value -15f);
    }

    public void StopMusic()
    {
        for (int i = 0; i < soundMusic.Length; i++)
        {
            soundMusic[i].Stop(); 
        }
    }
    public void PlayMain()
    {
        soundMusic[0].Play();
    }
    public void PlayGameMusic()
    {
        soundMusic[1].Play();
    }
    public void PlayBattle()
    {
        soundMusic[2].Play();
    }
    public void PlayEndMusic()
    {
        soundMusic[3].Stop();
    }
    public void PlaySFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();
        soundEffects[soundToPlay].pitch = Random.Range(.9f, 1.1f);
        currentSound = soundToPlay;
        soundEffects[soundToPlay].Play();
    }
    public void PlaySFXLoop(int soundToPlay)
    {
        soundEffects[soundToPlay].Play();
    }
    public void StopSFX()
    {
        soundEffects[6].Stop();
    }
    public void SoundsMute()
    {
        StopSFX();
        muted = true;
        generalMixer.GetFloat("Effects", out currentVol);
        generalMixer.SetFloat("Effects", - 1500f);
        float num = 0f;
        generalMixer.GetFloat("Effects", out num);
    }
    public void SoundsUnmute()
    {
        generalMixer.SetFloat("Effects", currentVol);
        muted = false;
    }
}
