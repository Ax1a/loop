using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTSManager : MonoBehaviour
{

    // this script is not in use right now, will delete if not needed.
    private AudioClip currentlyPlayingAudio;
    private AudioSource audioSource;

    public void OnButtonClick(AudioClip newAudioClip, Button button) {
        if (currentlyPlayingAudio != null) {
            StopAudio();
        }
        currentlyPlayingAudio = newAudioClip;
    }

    void StopAudio() {
        if (audioSource.isPlaying) {
            audioSource.Stop();
        }
    }

}
