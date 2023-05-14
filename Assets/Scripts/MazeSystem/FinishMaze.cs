using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FinishMaze : MonoBehaviour
{
    [SerializeField] MazeManager mazeManager;
    private bool canInteract = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (mazeManager != null)
            {
                mazeManager.CheckForWin();
                MazeUI.Instance.SetInteractionIndicator("Open Gate");
                canInteract = true;

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        canInteract = false;
        MazeUI.Instance.DisableInteractionIndicator();
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            if (mazeManager.canExit)
            {
                MazeUI.Instance.DisableInteractionIndicator();
                gameObject.SetActive(false);
            }
            else
            {
                MazeUI.Instance.ShowAlertPopup("Collect all the blocks first!");
                Debug.Log("Collect all the block first");
            }
        }
    }
}
