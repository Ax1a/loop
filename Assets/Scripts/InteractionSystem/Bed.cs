using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    Clock _clock;
    Energy _energy;
    [SerializeField] GameObject _time;
    [SerializeField] GameObject EnergyPanel;
    [SerializeField] private string _prompt;
    
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        _clock = _time.GetComponent<Clock>();
        _energy = EnergyPanel.GetComponent<Energy>();
        
        _energy.ResetEnergy();
        _clock.Minute = 0;
        _clock.Hour = 7;
        _clock.Day += 1;
        return true;
    }
}
