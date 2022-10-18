using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    Clock _clock;
    [SerializeField] GameObject _time;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        _clock = _time.GetComponent<Clock>();

        _clock.Minute = 0;
        _clock.Hour = 7;
        _clock.Day += 1;
        return true;
    }
}
