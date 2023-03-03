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

    // public void ToggleMusic ()
    // {
    //     AudioManager.Instance.ToggleMusic();
    // }

    // public void ToggleSfx ()
    // {
    //     AudioManager.Instance.ToggleSfx();
    // }
    public void MusicVolume ()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SfxVolume () 
    {
        AudioManager.Instance.SfxVolume(_sfxSlider.value);
    }

    public void ToggleMute ()
    {
        if (!isMute)
        {
            _musicSlider.value = 0;
            _sfxSlider.value = 0;
            AudioManager.Instance.MusicVolume(_musicSlider.value);
            AudioManager.Instance.SfxVolume(_musicSlider.value);
            isMute = true;
            muteBtn.image.sprite = mute;
             
        } else {
            _musicSlider.value = 1f;
            _sfxSlider.value = 1f;
            AudioManager.Instance.MusicVolume(_musicSlider.value);
            AudioManager.Instance.SfxVolume(_musicSlider.value);
            isMute = false;
            muteBtn.image.sprite = unmute;
        }
    }
}
