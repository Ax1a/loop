using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class FinishMaze : MonoBehaviour
// {
//     [SerializeField] MazeManager mazeManager;
//     private bool canInteract = false;

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject.tag == "Player")
//         {
//             if (mazeManager != null)
//             {
//                 mazeManager.CheckForWin();
//             }
//         }
//     }
//     private void OnTriggerExit(Collider other)
//     {
//         MazeUI.Instance.DisableInteractionIndicator();
//     }
// }
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
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Collect all the block first");
            }
        }
    }
}
