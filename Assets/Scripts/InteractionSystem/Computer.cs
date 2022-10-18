using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, Interactable
{
    OpenPanel Open;
    [SerializeField] GameObject _computer;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        Open = _computer.GetComponent<OpenPanel>();
        Open._OpenPanel();
        return true;
    }
}
