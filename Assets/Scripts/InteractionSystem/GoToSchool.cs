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
    [SerializeField] private GameObject Time;
    [SerializeField] private CanvasGroup IndicatorCanvas;
    [SerializeField] private GameObject questGiver;
    public bool isOpened = false;
    Clock _clock;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 5) {
            _clock = Time.GetComponent<Clock>();

            //hygiene: if 0, cant go to school
            if (HygieneSystem.Instance.currentHygiene == 0) {
            NPCDialogue.Instance.AddDialogue("*sniffs* ugh... I smell bad right now. I need to take a bath before going to school.", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
            return false;
            } 

            if (_clock.Hour < 12) {
                Energy.Instance.UseEnergy(5);
                //hygiene decrease
                HygieneSystem.Instance.DecreaseHygiene(40f);
                _clock.AddHour(hoursToSchool);
                StartCoroutine(PlayAnimation());   
            }
            else {
                NPCDialogue.Instance.AddDialogue("I think my class has already ended.", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
            }
            // condition to show that user is already late to class
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
        mainUI.SetActive(false);
        UIController.Instance.SetPanelActive(true);
        IndicatorCanvas.alpha = 0;

        yield return new WaitForSeconds(3f);

        clockAnimation.SetActive(false);
        mainUI.SetActive(true);
        UIController.Instance.SetPanelActive(false);
        IndicatorCanvas.alpha = 1;
        questGiver.SetActive(true);
        SaveGame.Instance.SaveGameState();
        _clock.UpdateLightRotation();
        _clock.UpdateLightSettings();
    }
}

