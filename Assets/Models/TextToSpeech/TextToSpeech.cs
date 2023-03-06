using UnityEngine;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private Image soundOnImg;
    [SerializeField] private Image soundOffImg;
    [SerializeField] private AudioClip audioClip;
    private bool isMuted = true;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
    }

    private void Start()
    {
        ChangeBtnImg();
    }

    public void OnButtonPress()
    {
        if (isMuted)
        {
            isMuted = false;
            soundOnImg.enabled = true;
            soundOffImg.enabled = false;
            audioSource.Play();
        }
        else
        {
            isMuted = true;
            soundOnImg.enabled = false;
            soundOffImg.enabled = true;
            audioSource.Stop();
        }
    }

    private void ChangeBtnImg()
    {
        if (isMuted)
        {
            soundOnImg.enabled = false;
            soundOffImg.enabled = true;
        }
        else
        {
            soundOnImg.enabled = true;
            soundOffImg.enabled = false;
        }
    }
}
