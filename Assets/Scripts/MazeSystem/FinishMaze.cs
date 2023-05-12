using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMaze : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
     {
        MazeManager mazeManager = other.GetComponent<MazeManager>();

        if (mazeManager != null)
        {
            mazeManager.CheckForWin();
        }
     }
}
