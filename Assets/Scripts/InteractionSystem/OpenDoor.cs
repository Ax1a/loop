using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour, Interactable
{

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool isOpened = false;

    public bool Interact(InteractObject interactor)
    {
        if(HygieneSystem.Instance.currentHygiene < 100){

        HygieneSystem.Instance.IncreaseHygiene();
        Debug.Log("test");
        }
        else{
                NPCDialogue.Instance.AddDialogue("I don't think I need a bath right now.", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
        }
        return true;
    }
}
