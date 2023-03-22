using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotGuide : MonoBehaviour
{
    /*
    Use AddDialogue to AddDialogue() to the List and ShowDialogue() to display the list
    Calling method: BotGuide.Instance.[function]
    Ex.
        Params: Dialogue
        AddDialogue("Check out these concise and easily understandable tips and useful information about IT, that will help you navigate the internet");
        ShowDialogue();
    */

    public List<string> _dialogues = new List<string>();

    [Header("Instances")]
    [SerializeField] private GameObject guideBot;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject tutorial;
    
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] dialogueTypingSoundClips;
    [Range (1,5)]
    [SerializeField] private int frequencyLevel = 2;
    [Range (-3,3)]
    [SerializeField] private float minPitch = 0.5f;
    [Range (-3,3)]
    [SerializeField] private float maxPitch = 3f;
    [Range (0,1)]
    [SerializeField] private float dialogueVolume = 1f;

    [SerializeField] private bool stopAudioSource;
    private AudioSource audioSource;

    PlayerController playerController;
    private bool isActive, canContinueNextLine, canSkip, playerSkipped;

    private Coroutine displayLineCoroutine;
    
    private static BotGuide _instance;
    public static BotGuide Instance {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<BotGuide>();

            if (_instance == null)
                Debug.Log("There is no Bot Guide");

            return _instance;
        }
    }

    private void OnEnable() {
        playerController = character.GetComponent<PlayerController>();
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = dialogueVolume;
    }

    private void Update() {
        if (canContinueNextLine) {
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _dialogues?.Count > 0) {
                _dialogues.RemoveAt(0);
                ShowDialogue();
            }
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && canSkip) {
            playerSkipped = true;
        }
    }

    public void AddDialogue(string dialogue) {
        _dialogues.Add(dialogue);
    }

    public void ShowDialogue() {
        tutorial.SetActive(false);
        // Display the first dialogue
        if (_dialogues?.Count > 0) {
            if (!UIController.Instance.popUpUIs.Contains(guideBot)) {
                UIController.Instance.EnqueuePopup(guideBot);
            }

            if (displayLineCoroutine != null) StopCoroutine(displayLineCoroutine);
            
            displayLineCoroutine = StartCoroutine(DisplayLine(_dialogues[0]));
            guideBot.SetActive(true);   
            isActive = true;
            if (animator.runtimeAnimatorController != null) animator.SetBool("isTalking", true);
        }   
        else {
            if (UIController.Instance.popUpUIs.Contains(guideBot)) {
                UIController.Instance.DequeuePopUp(guideBot);
            }
            guideBot.SetActive(false);   
            tutorial.SetActive(true);
            isActive = false;
        }
    }

    private IEnumerator DisplayLine(string line) {
        canContinueNextLine = false;
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueText.gameObject.SetActive(false);

        StartCoroutine(CanSkip());

        foreach (char letter in line.ToCharArray())
        {
            if (canSkip && playerSkipped) {
                playerSkipped = false;
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }
            PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
            dialogueText.maxVisibleCharacters++;

            yield return new WaitForSeconds(typingSpeed);
        }

        if (animator.runtimeAnimatorController != null) animator.SetBool("isTalking", false);
        canContinueNextLine = true;
        canSkip = false;
        continueText.gameObject.SetActive(true);
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter) {
        if (currentDisplayedCharacterCount % frequencyLevel == 0) {
            // Fix overlapping sound
            if (stopAudioSource) audioSource.Stop();

            AudioClip soundClip = null;

            int hashCode = currentCharacter.GetHashCode();
            // Sound clip
            int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
            soundClip = dialogueTypingSoundClips[predictableIndex];
            // Pitch
            int minPitchInt = (int) (minPitch * 100);
            int maxPitchInt = (int) (maxPitch * 100);
            int pitchRangeInt = maxPitchInt - minPitchInt;

            if (pitchRangeInt != 0) {
                int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                float predictablePitch = predictablePitchInt / 100f;
                audioSource.pitch = predictablePitch;
            }
            else {
                audioSource.pitch = minPitch;
            }

            // Play sound each letter
            audioSource.PlayOneShot(soundClip);
        }
    }

    private IEnumerator CanSkip()
    {
        canSkip = false; //Making sure the variable is false.
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    public bool guideIsActive() {
        return isActive;
    }
}
