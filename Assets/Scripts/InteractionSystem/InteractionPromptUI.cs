using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private GameObject _interactor;
    [SerializeField] private TextMeshProUGUI _promptText;
    InteractObject interact;

    void Start()
    {
        _uiPanel.SetActive(false);
        interact = _interactor.GetComponent<InteractObject>();
    }

    public bool isDisplayed = false;
    public void SetUp(string promptText)
    {
        _promptText.text = "[" + interact.getInteractKey() + "] " + promptText;
        _uiPanel.SetActive(true);
        isDisplayed = true;
    }

    public void Close()
    {
        _uiPanel.SetActive(false);
        isDisplayed = false;
    }
}
