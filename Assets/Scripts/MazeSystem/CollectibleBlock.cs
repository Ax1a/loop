using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBlock : MonoBehaviour
{
    public GameObject QuizPanel;
    public MazeQuizManager mazeQuizManager;
    public bool collected = false;
    [SerializeField] MazeManager mazeManager;
    private bool canInteract = false;

    void OnEnable()
    {
        collected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // mazeManager = other.GetComponent<MazeManager>();
            if (mazeManager != null)
            {
                if (QuizPanel != null)
                {
                    if (!QuizPanel.activeSelf) // check if quiz panel is not active
                    {
                        MazeUI.Instance.SetInteractionIndicator("Collect Block");
                        canInteract = true;
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
    }

    private void OnTriggerExit(Collider other) {
        canInteract = false;
        MazeUI.Instance.DisableInteractionIndicator();
    }

    private void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.E) && canInteract) {
            QuizPanel.SetActive(true);
            gameObject.SetActive(false);
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
    public void NotCollected()
    {
        if (gameObject != null)
            gameObject.SetActive(true);
    }
}
