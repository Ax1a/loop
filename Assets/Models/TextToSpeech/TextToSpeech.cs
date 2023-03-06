using UnityEngine;
using UnityEngine.UI;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private Image soundOnImg;
    [SerializeField] private Image soundOffImg;
    [SerializeField] private AudioClip audioClip;
    private bool isMuted = true;
    private bool isPlaying = false;
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
            isPlaying = true;
        }
        else
        {
            isMuted = true;
            soundOnImg.enabled = false;
            soundOffImg.enabled = true;
            audioSource.Stop();
            isPlaying = false;
        }
    }

    private void ChangeBtnImg()
    {
        if (isMuted || !isPlaying)
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

    private void Update()
    {
        if (!audioSource.isPlaying && isPlaying)
        {
            isMuted = true;
            isPlaying = false;
            ChangeBtnImg();
        }
    }
}
