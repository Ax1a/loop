using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable
{
    [SerializeField] private Clock timeAndDate;
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject sleepConfirmation;
    
    public string InteractionPrompt => _prompt;
    
    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 5) {

            //hygiene: if 0, cant sleep
            if (HygieneSystem.Instance.currentHygiene == 0) {
            NPCDialogue.Instance.AddDialogue("Yikes, I smell so bad right now. I should take a shower before going to bed!", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
            return false;
            }        
            
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
