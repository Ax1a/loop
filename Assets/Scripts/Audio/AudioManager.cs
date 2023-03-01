using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    void Awake () 
    {
        if (Instance != null) 
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start ()
    {
        PlayMusic ("Theme");
    }
    public void PlayMusic (string name)
    {
        Sounds s = Array.Find(musicSounds, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else 
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx (string name)
    {
        Sounds s = Array.Find(musicSounds, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
           sfxSource.PlayOneShot(s.clip);
      
        }

    }

    public void ToggleMusic  ()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSfx  ()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume  (float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume  (float volume)
    {
        sfxSource.volume = volume;
    }
    


}
