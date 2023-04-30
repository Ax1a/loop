using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour, Interactable
{
    OpenPanel Open;
    [SerializeField] GameObject _mainCam;
    [SerializeField] GameObject _computerCanvas;
    [SerializeField] Canvas _hud;
    [SerializeField] GameObject _interactOverlay;

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;
    public bool isOpened = false;
    bool _computerOpenedBefore; 

    public bool Interact(InteractObject interactor)
    {
        if (DataManager.GetTutorialProgress() >= 5) {
            // Disable HUD (Fix error when disabled)
            _interactOverlay.SetActive(false);
            _hud.enabled = false;
            _mainCam.SetActive(false);
            // Enable the Computer Camera
            CameraManager.Instance.ToggleTransitionCamera("CameraComputer");

            // // Disable the Camera
            if (isOpened == false) {
                UIController.Instance.SetPanelActive(true);
                StartCoroutine(OpenPanelDelay());
            }
            return true;
        }

        return false;
    }

    IEnumerator OpenPanelDelay() {
        yield return new WaitForSeconds(.96f);

        isOpened = true;
        _computerCanvas.SetActive(true);

        _computerOpenedBefore = PlayerPrefs.GetInt("ComputerOpenedBefore", 0) == 1;
        
        //checks if computer canvas is opened for the first time
        if(!_computerOpenedBefore)
        {
            Debug.Log("test");
            yield return new WaitForSeconds(3.3f);
            PlayerPrefs.SetInt("ComputerOpenedBefore", 1); 
            BotGuide.Instance.AddDialogue("Hello there! Welcome to your first time opening your computer in the game."); 
            BotGuide.Instance.AddDialogue("All programming lessons from your chosen language will be access in this computer!"); 
            BotGuide.Instance.AddDialogue("So try your best to finish all lessons!"); 
            BotGuide.Instance.ShowDialogue();
        }
    }
}
