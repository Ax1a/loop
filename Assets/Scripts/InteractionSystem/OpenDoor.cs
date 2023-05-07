using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDoor : MonoBehaviour, Interactable
{

    [SerializeField] private string _prompt;
    [SerializeField] private GameObject bathPopup;
    [SerializeField] private TextMeshProUGUI bathText;
    bool bathing = false;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        if(HygieneSystem.Instance.currentHygiene < 100){
            StartCoroutine(TakeBath());
        }
        else{
            NPCDialogue.Instance.AddDialogue("I don't think I need a bath right now.", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
        }
        return true;
    }

    private IEnumerator TakeBath() {
        UIController.Instance.SetPanelActive(true);
        AudioManager.Instance.PlaySfx("Shower");
        bathing = true;
        bathPopup.SetActive(true);
        StartCoroutine (ShowBathingText());
        yield return new WaitForSeconds(4f);

        bathing = false;
        bathPopup.SetActive(false);
        UIController.Instance.SetPanelActive(false);
        HygieneSystem.Instance.IncreaseHygiene();
        NPCDialogue.Instance.AddDialogue("Ah, this warm bath is just what I needed to relax and refresh myself.", DataManager.GetPlayerName());
        NPCDialogue.Instance.ShowDialogue();
    }

    private IEnumerator ShowBathingText() {
        string dots = "";
        while (bathing) {
            if (dots.Length > 3) dots = "";
            bathText.text = "Taking a bath" + dots;
            dots += ".";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
