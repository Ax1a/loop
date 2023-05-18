using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeUI : MonoBehaviour
{
    public TextMeshProUGUI blockText;
    public MazeManager mazeManager;
    public MazePlayerMovement movementManager;
    public GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactIndicator;
    [SerializeField] private TextMeshProUGUI alertTxt;
    public static MazeUI Instance;
    int collectedBlocks;
    int goalBlockCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        mazeManager.OnWin.AddListener(OnWin);
        // goalBlockCount = mazeManager.TotalBlockItemCount;
        UpdateCollectedText(mazeManager);
    }
    public void UpdateCollectedText(MazeManager mazeManager)
    {
        collectedBlocks = mazeManager.BlockItemCount;
        goalBlockCount = mazeManager.TotalBlockItemCount;
        blockText.text = collectedBlocks + " / " + goalBlockCount;
    }

    private void OnWin()
    {
        // Other actions if the player collected all the blocks
        //Activate Quiz interaction panel
        interactionPanel.SetActive(true);
        Debug.Log("You win!");
    }
    public void OnReset()
    {
        collectedBlocks = 0;
        goalBlockCount = mazeManager.TotalBlockItemCount;
        blockText.text = collectedBlocks + " / " + goalBlockCount;
    }

    public void SetInteractionIndicator(string text)
    {
        interactIndicator.text = text;
        interactIndicator.transform.parent.gameObject.SetActive(true);
    }

    public void DisableInteractionIndicator()
    {
        interactIndicator.transform.parent.gameObject.SetActive(false);
    }

    public void ShowAlertPopup(string text) {
        alertTxt.text = text;
        GameObject parent = alertTxt.transform.parent.gameObject;

        parent.SetActive(true);

        Image parentImage = parent.GetComponent<Image>();
        if (parentImage != null)
        {
            Color red = new Color(160f / 255f, 17f / 255f, 21f / 255f, 1f);
            parentImage.color = red;
        }
    }
    public void ShowSuccessPopup(string text)
    {
        alertTxt.text = text;
        GameObject parent = alertTxt.transform.parent.gameObject;

        parent.SetActive(true);

        Image parentImage = parent.GetComponent<Image>();
        if (parentImage != null)
        {
            Color green = new Color(17f / 255f, 160f / 255f, 19f / 255f, 1f);
            parentImage.color = green;
        }
    }
}
