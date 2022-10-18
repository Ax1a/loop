using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public string InteractionPrompt { get; }
    public bool Interact (InteractObject interactor);
}
