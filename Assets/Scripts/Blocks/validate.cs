using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class validate : MonoBehaviour
{
    private TMP_InputField input;
    public GameObject errorMessage;
    public TextMeshProUGUI output;
    public Transform simpleBlock;
    //public Button btn;
    public bool isValid;
    public static validate Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        input = simpleBlock.GetComponentInChildren<TMP_InputField>();
    }
    public void valid()
    {
        int _currPts = LessonDragDropValidation.Instance._currPts;
        int ptsToWin = LessonDragDropValidation.Instance.ptsToWin;

        if (input.text.Length != 0 && _currPts >= ptsToWin)
        {
            errorMessage.SetActive(false);
            output.text = input.text;
            isValid = true;
            NPCDialogue.Instance.AddDialogue("Great! Seems I understand the lesson", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
        }
        else
        {
            errorMessage.SetActive(true);
            isValid = false;
            NPCDialogue.Instance.AddDialogue("I think I missed something", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
        }
    }
    public void removeError()
    {
        if (errorMessage.activeSelf)
        {
            errorMessage.SetActive(false);
        }
    }

    public void ClearOutput()
    {
        output.text = "";
        input.text = "";
        if (errorMessage.activeSelf)
        {
            errorMessage.SetActive(false);
        }
    }
}
