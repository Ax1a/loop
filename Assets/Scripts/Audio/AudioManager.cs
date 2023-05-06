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
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SfxVolume";

    void Start () 
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
    }

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
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        // Save music volume to PlayerPrefs
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
        // Save sfx volume to PlayerPrefs
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
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
