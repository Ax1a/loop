using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenDoor : MonoBehaviour, Interactable
{

    [SerializeField] private string _prompt;
    [SerializeField] private GameObject bathPopup;
    [SerializeField] private TextMeshProUGUI bathText;
    [SerializeField] private GameObject shirt;
    bool bathing = false;
    public string InteractionPrompt => _prompt;
    private Renderer shirtRenderer;

    private void Start() {
        shirtRenderer = shirt.GetComponent<Renderer>();
    }

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
        UIController.Instance.onTopCanvas.SetActive(false);
        AudioManager.Instance.PlaySfx("Shower");
        bathing = true;
        bathPopup.SetActive(true);
        StartCoroutine (ShowBathingText());
        yield return new WaitForSeconds(4f);

        bathing = false;
        bathPopup.SetActive(false);
        UIController.Instance.SetPanelActive(false);
        UIController.Instance.onTopCanvas.SetActive(true);
        HygieneSystem.Instance.IncreaseHygiene();
        NPCDialogue.Instance.AddDialogue("Ah, this warm bath is just what I needed to relax and refresh myself.", DataManager.GetPlayerName());
        NPCDialogue.Instance.ShowDialogue();

        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        shirtRenderer.material.color = randomColor;
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
