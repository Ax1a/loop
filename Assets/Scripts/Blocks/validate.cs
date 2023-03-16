using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Validate : MonoBehaviour
{
    private TMP_InputField input;
    public GameObject errorMessage;
    public TextMeshProUGUI output;
    public Transform simpleBlock;
    //public Button btn;
    public bool isValid;
    public static Validate Instance;
    
    void Awake () 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void valid()
    {
        input = simpleBlock.GetComponentInChildren<TMP_InputField>();

        if(input.text.Length != 0)
        {
            errorMessage.SetActive(false);
            output.text = input.text;
            NPCDialogue.Instance.AddDialogue("Great! Seems I understand the lesson", DataManager.GetPlayerName()); 
            NPCDialogue.Instance.ShowDialogue();
        }
        else
        {
            errorMessage.SetActive(true);
            NPCDialogue.Instance.AddDialogue("I think I should input some values", DataManager.GetPlayerName()); 
            NPCDialogue.Instance.ShowDialogue();    
        }
    }
    public void removeError()
    {
        if(errorMessage.activeSelf)
        {
        errorMessage.SetActive(false);

        }
    }

    public void ToggleBool ()
    {
        if (isValid)
        {
            isValid = false;
        }
        else 
        {
            isValid = true;
        }
    }
}
