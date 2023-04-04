using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPC : MonoBehaviour, Interactable
{
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject highlightGuide;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 3) {
            UIController.Instance.ToggleUI("AssemblePopup");
            StartCoroutine(DelayEnqueue());
            
            return true;
        }

        return false;
    }

    private IEnumerator DelayEnqueue() {
        yield return new WaitForSeconds(.3f);
        UIController.Instance.EnqueuePopup(highlightGuide);
    }
}
