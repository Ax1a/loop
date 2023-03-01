using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotGuide : MonoBehaviour
{
    public List<string> _dialogues = new List<string>();

    [Header("Instances")]
    [SerializeField] private GameObject guideBot;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI continueText;
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;
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
        // Display the first dialogue
        if (_dialogues?.Count > 0) {
            // dialogueText.text = _dialogues[0];
            if (displayLineCoroutine != null) StopCoroutine(displayLineCoroutine);
            
            displayLineCoroutine = StartCoroutine(DisplayLine(_dialogues[0]));
            guideBot.SetActive(true);   
            isActive = true;
            if (animator.runtimeAnimatorController != null) animator.SetBool("isTalking", true);
        }   
        else {
            guideBot.SetActive(false);   
            isActive = false;
        }
    }

    private IEnumerator DisplayLine(string line) {
        canContinueNextLine = false;
        dialogueText.text = "";
        continueText.gameObject.SetActive(false);

        StartCoroutine(CanSkip());

        foreach (char letter in line.ToCharArray())
        {
            // if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && canSkip) {
            //     dialogueText.text = line;
            //     break;
            // }

            if (canSkip && playerSkipped) {
                playerSkipped = false;
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if (animator.runtimeAnimatorController != null) animator.SetBool("isTalking", false);
        canContinueNextLine = true;
        canSkip = false;
        continueText.gameObject.SetActive(true);
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
