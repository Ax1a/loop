using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LessonInteractions : MonoBehaviour
{
    [SerializeField] private string[] correctAnswers;
    public TMP_InputField[] userAnswer;
    string _playerName;

    void Start()
    {
        _playerName = DataManager.GetPlayerName();
    }
    public void CheckAnswer()
    {
        bool allCorrect = true;
        bool allEmpty = false;
        string incorrectAnswers = "";
        for (int i = 0; i < userAnswer.Length; i++)
        {
            //To - fix : NPC when all input box are empty.
            if (userAnswer[i].text == "")
            {
                allEmpty = true;
            }

            if (userAnswer[i].text != correctAnswers[i])
            {
                allCorrect = false;
                //Optional: getting the incorrect answer.
                incorrectAnswers += "Question " + (i + 1) + " is incorrect.\n";
            }
        }

        if (allEmpty)
        {
            NPCCall("I think I should input some values...");
            allEmpty = false;
        }

        if (allCorrect)
        {
            NPCCall("Great! Seems I understand the lesson");
            Debug.Log("All are correct");
        }
        else
        {
            NPCCall("I better review the lesson again! :<");
            Debug.Log("Try again :<");
        }

    }

    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
