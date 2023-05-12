using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBlock : MonoBehaviour
{
    public GameObject QuizPanel;
    public MazeQuizManager mazeQuizManager;
    public bool collected = false;
    [SerializeField] MazeManager mazeManager;

    void OnEnable()
    {
        collected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        mazeManager = other.GetComponent<MazeManager>();
        if (mazeManager != null)
        {
            if (QuizPanel != null)
            {
                if (!QuizPanel.activeSelf) // check if quiz panel is not active
                {
                    QuizPanel.SetActive(true);
                    gameObject.SetActive(false);
                }
                else if (mazeQuizManager.currentPoints == mazeQuizManager.pointsToWin && !collected) // check if player has answered the quiz
                {
                    collected = true;
                    mazeQuizManager.Correct(); // trigger the correct answer logic
                }
                else
                {
                    Debug.Log("try again");
                }
            }
        }
    }


    public void Collect()
    {
        if (!collected)
        {
            if (mazeManager != null)
            {
                mazeManager.BlocksCollected();
            }
            collected = true;
            gameObject.SetActive(false);
            Debug.Log("Collected");
        }
    }

    public void NoTCollected()
    {
        gameObject.SetActive(true);

    }
}
