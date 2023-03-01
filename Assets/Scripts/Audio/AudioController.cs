using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void ToggleMusic ()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSfx ()
    {
        AudioManager.Instance.ToggleSfx();
    }

    public void MusicVolume ()
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SfxVolume () 
    {
        AudioManager.Instance.SfxVolume(_sfxSlider.value);
    }

}
