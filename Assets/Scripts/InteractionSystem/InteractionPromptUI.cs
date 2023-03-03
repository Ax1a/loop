using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private TextMeshProUGUI _promptKeyText;

    void Start()
    {
        _uiPanel.SetActive(false);
    }

    public bool isDisplayed = false;
    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        _promptKeyText.text = InputManager.Instance.interact.ToString();
        _uiPanel.SetActive(true);
        isDisplayed = true;
        AudioManager.Instance.PlaySfx("Interact");
    }

    public void Close()
    {
        _uiPanel.SetActive(false);
        isDisplayed = false;
    }
}
