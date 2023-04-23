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

    [Header("Varaibles for Character manipulation")]
    [SerializeField] private TextMeshProUGUI CM_output;
    [SerializeField] TMP_InputField textToConvert, oldVal, newVal, indexOf_Input;

    void Start()
    {
        _playerName = DataManager.GetPlayerName();

        UpdateAllCorrect();

        if (userAnswer != null && userAnswer.Length > 0)
        {
            //used to check the input of the users each time they change
            foreach (TMP_InputField inputField in userAnswer)
            {
                inputField.onValueChanged.AddListener(delegate { UpdateAllCorrect(); });
            }
        }
    }
    private void UpdateAllCorrect()
    {
        allCorrect = true;

        for (int i = 0; i < userAnswer.Length; i++)
        {
            if (userAnswer[i].text != correctAnswers[i])
            {
                allCorrect = false;
                break;
            }
        }
    }
    public void CheckAnswer()
    {
        if (userAnswer != null && userAnswer.Length > 0)
        {
            for (int i = 0; i < userAnswer.Length; i++)
            {
                //To - fix : NPC when all input box are empty.
                if (userAnswer[i].text == "")
                {
                    NPCCall("I think I should input some values...");
                    return;
                }
            }
        }
    }
    public void CharacterManipulation(string operation)
    {
        CheckAnswer();
        int currentPts = LessonDragDropValidation.Instance.CurrPts;
        int ptsTowin = LessonDragDropValidation.Instance.ptsToWin;

        if (allCorrect && currentPts >= ptsTowin)
        {
            string convertedText;
            switch (operation)
            {
                case "toUpperCase":
                    convertedText = textToConvert.text.ToUpper();
                    break;
                case "toLowerCase":
                    convertedText = textToConvert.text.ToLower();
                    break;
                case "length":
                    convertedText = textToConvert.text.Length.ToString();
                    break;
                case "replace":
                    convertedText = textToConvert.text.Replace(oldVal.text.ToString(), newVal.text.ToString());
                    break;
                case "indexOf":
                    convertedText = textToConvert.text.IndexOf(indexOf_Input.text.ToString()).ToString();
                    break;
                default:
                    convertedText = textToConvert.text;
                    break;
            }
            CM_output.text = convertedText;

            NPCCall("Great! I understand the lesson completely");
        }
        else if (currentPts < ptsTowin)
        {
            NPCCall("I should put the blocks in the right place!");
        }
        else
        {
            NPCCall("My answers may be correct but I need to follow the instructions...");
        }

    }

    public void ClearCMInputs()
    {
        textToConvert?.SetTextWithoutNotify("");
        oldVal?.SetTextWithoutNotify("");
        newVal?.SetTextWithoutNotify("");
        indexOf_Input?.SetTextWithoutNotify("");
        CM_output?.SetText("");
    }

    //This function is to remove
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
    public void ConsoleOutput()
    {
        //Clear the console
        ClearConsole();

        int currentPts = LessonDragDropValidation.Instance.CurrPts;
        int ptsTowin = LessonDragDropValidation.Instance.ptsToWin;

        CheckAnswer();

        if (allCorrect && currentPts >= ptsTowin)
        {
            NPCCall("Great! I understand the lesson completely");
            StartCoroutine(ActivateOutput());
        }
        else if (currentPts < ptsTowin)
        {
            NPCCall("I should put the blocks in the right place!");
        }
        else
        {
            NPCCall("My answers may be correct but I need to follow the instructions...");
        }
    }

    public void ClearConsole()
    {
        foreach (Transform childTransform in outputParent.transform)
        {
            // Get the child game object from the transform
            GameObject childObject = childTransform.gameObject;

            // Activate the child game object
            childObject.SetActive(false);
        }

    }
    public void ClearUserInput()
    {
        //Clear user input
        for (int i = 0; i < userAnswer.Length; i++)
        {
            userAnswer[i].text = "";
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
