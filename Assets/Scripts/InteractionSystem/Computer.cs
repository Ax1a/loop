using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, Interactable
{
    OpenPanel Open;
    [SerializeField] GameObject _computer;
    [SerializeField] GameObject _hud;
    [SerializeField] GameObject _interactOverlay;
    [SerializeField] GameObject compCamera;

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool isOpened = false;

    public bool Interact(InteractObject interactor)
    {
        // Disable HUD (Fix error when disabled)
        // _hud.gameObject.SetActive(false);
        _interactOverlay.SetActive(false);

        // Enable the Camera
        compCamera.gameObject.SetActive(true);
        // // Disable the Camera
        // compCamera.gameObject.SetActive(false);
        if (isOpened == false) {
            UIController.Instance.SetPanelActive(true);
            StartCoroutine(OpenPanelDelay());   
        }
        return true;
    }

    IEnumerator OpenPanelDelay() {
        yield return new WaitForSeconds(1);

        isOpened = true;
        Open = _computer.GetComponent<OpenPanel>();
        Open._OpenPanel();
    }
}
