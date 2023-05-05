using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    [SerializeField] private Clock timeAndDate;
    [SerializeField] GameObject EnergyPanel;
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject sleepConfirmation;
    
    public string InteractionPrompt => _prompt;
    
    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 5) {

            if (timeAndDate.Hour < 17) {
                sleepConfirmation.SetActive(true);
                UIController.Instance.SetPanelActive(true);
            }
            else {
                StartCoroutine(SleepingPopUp.Instance.ShowPopUp());
            }

            return true;
        }
        else if (DataManager.GetTutorialProgress() == 3) {
            NPCDialogue.Instance.AddDialogue("I'd better build my computer before I do anything else.", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
        }

        return false;
    }

    public void CancelSleep() {
        sleepConfirmation.SetActive(false);
        UIController.Instance.SetPanelActive(false);
    }

    public void ContinueSleep() {
        sleepConfirmation.SetActive(false);
        StartCoroutine(SleepingPopUp.Instance.ShowPopUp());
    }

}
