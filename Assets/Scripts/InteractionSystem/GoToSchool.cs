using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToSchool : MonoBehaviour, Interactable
{

    [SerializeField] private string _prompt;
    [SerializeField] private int hoursToSchool;
    [SerializeField] private GameObject clockAnimation;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject Time;
    public bool isOpened = false;
    Clock _clock;
    public string InteractionPrompt => _prompt;

    public bool Interact(InteractObject interactor)
    {
        _clock = Time.GetComponent<Clock>();
        _clock.Hour += hoursToSchool;
        StartCoroutine(PlayAnimation());   
        return true;
    }

    private IEnumerator PlayAnimation() {
        clockAnimation.SetActive(true);
        mainUI.SetActive(false);
        UIController.Instance.SetPanelActive(true);

        yield return new WaitForSeconds(3f);

        clockAnimation.SetActive(false);
        mainUI.SetActive(true);
        UIController.Instance.SetPanelActive(false);
        SaveGame.Instance.SaveGameState();
    }
}

