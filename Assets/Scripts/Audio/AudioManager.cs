using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;
    public Sounds[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private Sounds currentMusic;

    void Awake()
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

    public void PlayMusic(string name)
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
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySfx(string name)
    {
        Sounds s = Array.Find(sfxSounds, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMute ()
    {
        musicSource.volume = 0;
        sfxSource.volume = 0;
    }
    // public void ToggleMusic()
    // {
    //     musicSource.mute = !musicSource.mute;
    // }
    // public void ToggleSfx()
    // {
    //     sfxSource.mute = !sfxSource.mute;
    // }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0: // First scene
                PlayMusic("MainMenu");
                break;
            case 1: // Second scene
                PlayMusic("Day");
                break;
            case 2: // Third scene
                PlayMusic("CutScene");
                break;
        }
    }
}
