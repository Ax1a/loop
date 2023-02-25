using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    Energy _energy;
    [SerializeField] GameObject EnergyPanel;
    [SerializeField] private string _prompt;
    
    public string InteractionPrompt => _prompt;
    
    public bool Interact(InteractObject interactor)
    {
        _energy = EnergyPanel.GetComponent<Energy>();
        _energy.ResetEnergy();
        
        StartCoroutine(SleepingPopUp.Instance.ShowPopUp());

        return true;
    }

}
