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
    private float currentVolSFX;
    private float savedVolSFX;
    private float savedVolMUS;
    private float savedVolGEN;
    public bool muted=false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        generalSldr.minValue = -80;
        generalSldr.maxValue = 10;
        
        musicSldr.minValue = -80;
        musicSldr.maxValue = 10;

        effectsSldr.minValue = -80;
        effectsSldr.maxValue = 10;

        LoadVolume();

        effectsSldr.value = effectsVol;
        musicSldr.value = musicVol;
        generalSldr.value = generalVol;
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
        generalMixer.GetFloat("Effects", out currentVolSFX);
        generalMixer.SetFloat("Effects", - 1500f);
        float num = 0f;
        generalMixer.GetFloat("Effects", out num);
    }
    public void SoundsUnmute()
    {
        generalMixer.SetFloat("Effects", currentVolSFX);
        muted = false;
    }
    
    public void SaveVolume()
    {
        generalMixer.GetFloat("Effects", out savedVolSFX);
        PlayerPrefs.SetFloat("VolEffects", savedVolSFX);

        generalMixer.GetFloat("Music", out savedVolMUS);//
        PlayerPrefs.SetFloat("VolMusic", savedVolMUS);

        generalMixer.GetFloat("General", out savedVolGEN);//
        PlayerPrefs.SetFloat("VolGeneral", savedVolGEN);

        Debug.Log("Saved :" + savedVolSFX + "FX, " + savedVolMUS + "M, " +savedVolGEN+"G");
    }
    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey("VolEffects"))
        {
            savedVolSFX = PlayerPrefs.GetFloat("VolEffects");
            generalMixer.SetFloat("Effects", savedVolSFX);

            savedVolMUS = PlayerPrefs.GetFloat("VolMusic");//
            generalMixer.SetFloat("Music", savedVolMUS);

            savedVolGEN = PlayerPrefs.GetFloat("VolGeneral");//
            generalMixer.SetFloat("General", savedVolGEN);
            Debug.Log("Loaded :" + savedVolSFX + "FX, " + savedVolMUS + "M, " + savedVolGEN+"G");
        }
    }
}
