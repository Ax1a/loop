using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BotGuide : MonoBehaviour
{
    public List<string> _dialogues = new List<string>();
    [SerializeField] private GameObject guideBot;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool isActive;
    

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

    public void AddDialogue(string dialogue) {
        _dialogues.Add(dialogue);
    }

    public void ShowDialogue() {
        // Display the first dialogue
        if (_dialogues?.Count > 0) {
            dialogueText.text = _dialogues[0];
            guideBot.SetActive(true);   
            isActive = true;
        }   
        else {
            guideBot.SetActive(false);   
            isActive = false;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && _dialogues?.Count > 0) {
            _dialogues.RemoveAt(0);
            ShowDialogue();
        }
    }

    public bool guideIsActive() {
        return isActive;
    }
}
