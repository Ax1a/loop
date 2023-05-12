using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBlock : MonoBehaviour
{
    public GameObject QuizPanel;
    public MazeQuizManager mazeQuizManager;

    private void OnTriggerEnter(Collider other)
    {
        MazeManager mazeManager = other.GetComponent<MazeManager>();

        if (mazeManager != null)
        {
            if (QuizPanel != null)
            {
                if (!QuizPanel.activeSelf) // check if quiz panel is not active
                {
                    QuizPanel.SetActive(true);
                }
                else if (mazeQuizManager.currentPoints == mazeQuizManager.pointsToWin) // check if player has answered the quiz
                {
                    mazeManager.BlocksCollected();
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("try again");
                }
            }
        }

    }
}
