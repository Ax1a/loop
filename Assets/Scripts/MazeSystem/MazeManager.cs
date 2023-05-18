using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    void Awake()
    {
        TotalBlockItemCount = BlockItemsParent.transform.childCount;
    }
    void Start()
    {
        // TotalBlockItemCount = BlockItemsParent.transform.childCount;
        lifeSystem = GetComponent<LifeSystem>();
        StartGamePanel?.SetActive(true);
    }
    public void BlocksCollected()
    {
        BlockItemCount++;
        OnBlockItemCollected.Invoke(this);

    }

    public void CheckForWin()
    {
        if (BlockItemCount == TotalBlockItemCount)
        {
            // OnWin.Invoke();
            canExit = true;
        }
    }

    public void StartGame()
    {
        StartGamePanel?.SetActive(false);
    }
    public void ResetGame()
    {
        // OLD FUNCTIONS FOR RESET GAME
        // BlockItemCount = 0;
        // ActivateBlockItems();
        // movementManager.ResetPosition();
        // MazeUI.Instance.OnReset();
        // lifeSystem.StartGame();

        // RESETING SCENE
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
