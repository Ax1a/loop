using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeUI : MonoBehaviour
{   
    public TextMeshProUGUI blockText;
    public MazeManager mazeManager;
    public MovementManager movementManager;

    public GameObject interactionPanel;
    void Start()
    {
        mazeManager.OnWin.AddListener(OnWin);
    }
    public void UpdateCollectedText (MazeManager mazeManager)
    {
        blockText.text = mazeManager.BlockItemCount.ToString();
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
        blockText.text = "0";
        interactionPanel.SetActive(false);
        mazeManager.ResetGame();
        movementManager.ResetPosition();
    }

}
 