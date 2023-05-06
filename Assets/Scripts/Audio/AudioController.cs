using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    private bool isMute = false;
    public Sprite mute;
    public Sprite unmute;
    public Button muteBtn;

    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);
        _musicSlider.value = musicVolume;
        _sfxSlider.value = sfxVolume;
        AudioManager.Instance.SetMusicVolume(musicVolume);
        AudioManager.Instance.SetSfxVolume(sfxVolume);

        int isMuteValue = PlayerPrefs.GetInt("isMute", 0);
        if (isMuteValue == 1)
        {
            ToggleMute();
        }
    }
    public void MusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(_musicSlider.value);
    }

    public void SfxVolume()
    {
        AudioManager.Instance.SetSfxVolume(_sfxSlider.value);
    }
    public void ToggleMute()
    {
        if (!isMute)
        {
            _musicSlider.value = 0;
            _sfxSlider.value = 0;
            AudioManager.Instance.MusicVolume(_musicSlider.value);
            AudioManager.Instance.SfxVolume(_musicSlider.value);
            isMute = true;
            muteBtn.image.sprite = mute;
            PlayerPrefs.SetInt("isMute", 1);
        }
        else
        {
            _musicSlider.value = 1f;
            _sfxSlider.value = 1f;
            AudioManager.Instance.MusicVolume(_musicSlider.value);
            AudioManager.Instance.SfxVolume(_musicSlider.value);
            isMute = false;
            muteBtn.image.sprite = unmute;
            PlayerPrefs.SetInt("isMute", 0);
        }
        PlayerPrefs.Save();
    }

}
