using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    #region Fields

    public static SFXPlayer Instance;

    public AudioSource musicSource;
    public AudioSource oneShotSource;
    
    [SerializeField] private SFXData music;

    #endregion

    #region UnityCallbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (musicSource && music)
        {
            PlayMusic();
        }
    }

    #endregion

    #region Methods

    private void PlayMusic()
    {
        musicSource.clip = music.clip;
        musicSource.volume = music.volume;
        musicSource.pitch = music.pitch;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayOneShot(string sfxKey)
    {
        SFXData sfxData = SFXManager.Instance.GetSFXDataFromKey(sfxKey);
        if (sfxData)
        {
            oneShotSource.pitch = sfxData.pitch;
            oneShotSource.PlayOneShot(sfxData.clip, sfxData.volume);
        }
    }

    #endregion
}
