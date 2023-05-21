using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDialogueOnEnable : MonoBehaviour
{
    [SerializeField] private string[] dialogues;

    [Header ("If One Time Only")]
    [SerializeField] private bool oneTimeOnly;
    [SerializeField] private string playerKeyPref;
    private bool oneTimeCheck;

    private void OnEnable() {
        oneTimeCheck = PlayerPrefs.GetInt(playerKeyPref, 0) == 1;

        if (oneTimeOnly && oneTimeCheck) return;
        foreach (var dialogue in dialogues)
        {
            BotGuide.Instance.AddDialogue(dialogue);
        }

        PlayerPrefs.SetInt(playerKeyPref, 1); 
        BotGuide.Instance.ShowDialogue();
    }
}
