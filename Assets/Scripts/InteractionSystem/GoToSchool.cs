using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSchool : MonoBehaviour, Interactable
{

    [SerializeField] private string _prompt;
    [SerializeField] private int hoursToSchool;
    [SerializeField] private int energyCost;
    [SerializeField] private GameObject clockAnimation;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private Clock timeAndDate;
    [SerializeField] private CanvasGroup IndicatorCanvas;
    [SerializeField] private GameObject questGiver;
    
    [Header ("Character Objects")]
    [SerializeField] private GameObject bagMain;
    [SerializeField] private GameObject bagSupp;
    [SerializeField] private GameObject smokeEffect;
    public bool isOpened = false;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 5) {
            if ((int)timeAndDate.weekDay >= 5) {
                string day = "";
                day = (int)timeAndDate.weekDay == 5 ? "Saturday" : "Sunday";
                NPCDialogue.Instance.AddDialogue("I don't have classes on " + day + ". Time to relax!", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
                return false;
            }

            //hygiene: if 0, cant go to school
            if (HygieneSystem.Instance.currentHygiene == 0) {
                NPCDialogue.Instance.AddDialogue("*sniffs* ugh... I smell bad right now. I need to take a bath before going to school.", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
                return false;
            } 

            // Go to school
            if (timeAndDate.Hour >= 10 && timeAndDate.Hour <= 12) {
                Energy.Instance.UseEnergy(5);
                //hygiene decrease
                HygieneSystem.Instance.DecreaseHygiene(40f);
                timeAndDate.SetHourAndMinute(hoursToSchool, 30);
                bagMain.SetActive(true);
                bagSupp.SetActive(true);
                smokeEffect.SetActive(true);
                StartCoroutine(PlayAnimation());   
            }
            else if (timeAndDate.Hour < 10) {
                NPCDialogue.Instance.AddDialogue("It's too early to go to school.", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
            }
            else {
                // condition to show that user is already late to class
                NPCDialogue.Instance.AddDialogue("I think my class has already ended.", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
            }
            return true;
        }
        else if (DataManager.GetTutorialProgress() >= 3) {
            NPCDialogue.Instance.AddDialogue("I'd better build my computer before I do anything else.", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
        }

        return false;
    }

    private IEnumerator PlayAnimation() {
        clockAnimation.SetActive(true);
        AudioManager.Instance.PlaySfx("School");
        mainUI.SetActive(false);
        UIController.Instance.SetPanelActive(false);
        IndicatorCanvas.alpha = 0;
        UIController.Instance.onTopCanvas.SetActive(false);

        yield return new WaitForSeconds(3f);

        clockAnimation.SetActive(false);
        mainUI.SetActive(true);
        UIController.Instance.SetPanelActive(false);
        UIController.Instance.onTopCanvas.SetActive(true);
        IndicatorCanvas.alpha = 1;
        questGiver.SetActive(true);
        SaveGame.Instance.SaveGameState();
        timeAndDate.UpdateLightRotation();
        DataManager.AddDaysAttended(1);
        timeAndDate.UpdateLightSettings();
        bagMain.SetActive(false);
        bagSupp.SetActive(false);
    }
}

