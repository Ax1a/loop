using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    /*
    Use AddDialogue to AddDialogue() to the List and ShowDialogue() to display the list
    Calling method: NPCDialogue.Instance.[function]
    Ex.
        Params: (Dialogue, NPC Name)
        AddDialogue("Check out these concise and easily understandable tips and useful information about IT, that will help you navigate the internet.y", DataManager.GetPlayerName());
        ShowDialogue();
    */
    public List<string> _dialogues = new List<string>();

    [Header("Instances")]
    [SerializeField] private GameObject npcDialogue;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private TextMeshProUGUI npcName;
    
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Audio")]
    [SerializeField] private AudioClip dialogueTypingSoundClips;
    [Range (1,5)]
    [SerializeField] private int frequencyLevel = 2;
    [Range (0,1)]
    [SerializeField] private float dialogueVolume = 1f;

    [SerializeField] private bool stopAudioSource;
    private AudioSource audioSource;

    private bool isActive, canContinueNextLine, canSkip, playerSkipped;

    private Coroutine displayLineCoroutine;
    
    private static NPCDialogue _instance;
    public static NPCDialogue Instance {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<NPCDialogue>();

            if (_instance == null)
                Debug.Log("There is no Bot Guide");

            return _instance;
        }
    }

    private void OnEnable() {
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

    public void AddDialogue(string dialogue, string name) {
        _dialogues.Add(dialogue);
        npcName.text = name;
    }

    public void ShowDialogue() {
        // Display the first dialogue
        if (_dialogues?.Count > 0) {
            // dialogueText.text = _dialogues[0];
            if (displayLineCoroutine != null) StopCoroutine(displayLineCoroutine);
            
            displayLineCoroutine = StartCoroutine(DisplayLine(_dialogues[0]));
            npcDialogue.SetActive(true);   
            isActive = true;
        }   
        else {
            npcDialogue.SetActive(false);   
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
            PlayDialogueSound(dialogueText.maxVisibleCharacters);
            dialogueText.maxVisibleCharacters++;

            yield return new WaitForSeconds(typingSpeed);
        }

        canContinueNextLine = true;
        canSkip = false;
        continueText.gameObject.SetActive(true);
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount) {
        if (currentDisplayedCharacterCount % frequencyLevel == 0) {
            // Fix overlapping sound
            if (stopAudioSource) audioSource.Stop();

            // Play sound each letter
            audioSource.volume = dialogueVolume;
            audioSource.PlayOneShot(dialogueTypingSoundClips);
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
