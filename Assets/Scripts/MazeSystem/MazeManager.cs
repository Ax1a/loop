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
    public UnityEvent OnReset;

    void Start()
    {
        TotalBlockItemCount = BlockItemsParent.transform.childCount;
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
            OnWin.Invoke();
        }
        else
        {
            Debug.Log("Collect all the block first");
        }
    }
    public void ResetGame()
    {
        // To-do: Reset player initial position
        BlockItemCount = 0;
        ActivateBlockItems();


        OnReset.Invoke();
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
