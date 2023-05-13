using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MazeManager : MonoBehaviour
{
    public int BlockItemCount { get; private set; }
    public int TotalBlockItemCount { get; private set; }
    public GameObject BlockItemsParent;
    public UnityEvent<MazeManager> OnBlockItemCollected;
    public UnityEvent OnWin;
    // public UnityEvent OnReset;
    [HideInInspector] public bool canExit = false;
    public MazePlayerMovement movementManager;
    LifeSystem lifeSystem;
    [SerializeField] private GameObject StartGamePanel;

    void Start()
    {
        TotalBlockItemCount = BlockItemsParent.transform.childCount;
        lifeSystem = GetComponent<LifeSystem>();
        StartGamePanel?.SetActive(true);
    }
    public void BlocksCollected()
    {
        BlockItemCount++;
        OnBlockItemCollected.Invoke(this);
        Debug.Log("Item collected");
        // CheckForWin();
    }

    public void CheckForWin()
    {
        if (BlockItemCount == TotalBlockItemCount)
        {
            // OnWin.Invoke();
            canExit = true;
        }
    }

    public void StartGame ()
    {
        StartGamePanel?.SetActive(false);
    }
    public void ResetGame()
    {
        // To-do: Reset player initial position
        BlockItemCount = 0;
        ActivateBlockItems();
        movementManager.ResetPosition();
        MazeUI.Instance.OnReset();
        lifeSystem.StartGame();
        // OnReset.Invoke();
    }
    public void ActivateBlockItems()
    {
        if (BlockItemsParent != null)
        {
            foreach (Transform childTransform in BlockItemsParent.transform)
            {
                GameObject childObject = childTransform.gameObject;
                childObject.SetActive(true);
            }
        }
    }
}
