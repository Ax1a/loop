using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LessonInteractions : MonoBehaviour
{
    public GameObject outputParent;
    [SerializeField] private string[] correctAnswers;
    public TMP_InputField[] userAnswer;
    string _playerName;
    bool allCorrect = false;
    void Start()
    {
        _playerName = DataManager.GetPlayerName();
    }
    public void CheckAnswer()
    {
        allCorrect = true;
        string incorrectAnswers = "";
        for (int i = 0; i < userAnswer.Length; i++)
        {
            //To - fix : NPC when all input box are empty.
            if (userAnswer[i].text == "")
            {
                NPCCall("I think I should input some values...");
                return;
            }

            if (userAnswer[i].text != correctAnswers[i])
            {
                allCorrect = false;
                //Optional: getting the incorrect answer.
                incorrectAnswers += "Question " + (i + 1) + " is incorrect.\n";
            }
        }
        LessonDragDropValidation.Instance.IsCorrect();
        allCorrect = LessonDragDropValidation.Instance.isCorrect;
    }

    public void ListOutput()
    {
        if (allCorrect)
        {
            NPCCall("Great! I understand how to create a list!");
        }
        else
        {
            NPCCall("I better review the lesson again! :<");
        }
    }
    public void ForLoopOutput()
    {
        if (allCorrect)
        {
            StartCoroutine(ActivateOutput());
        }
    }

    public void ClearForLoopConsole()
    {
        foreach (Transform childTransform in outputParent.transform)
        {
            // Get the child game object from the transform
            GameObject childObject = childTransform.gameObject;

            // Activate the child game object
            childObject.SetActive(false);
        }
    }
    IEnumerator ActivateOutput()
    {
        // Iterate through all child game objects of the parentObject using foreach
        foreach (Transform childTransform in outputParent.transform)
        {
            // Get the child game object from the transform
            GameObject childObject = childTransform.gameObject;
            // Activate the child game object
            childObject.SetActive(true);
            // Wait for the specified delay before activating the next child object
            yield return new WaitForSeconds(0.5f);
        }
    }

    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }

}
