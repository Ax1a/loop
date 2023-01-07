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
        Debug.Log("test");
        return true;
    }
}
