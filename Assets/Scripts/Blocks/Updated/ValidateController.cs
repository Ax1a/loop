using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateController : MonoBehaviour
{
    [SerializeField] private int requiredPoints;
    [SerializeField] private TextMeshProUGUI consoleTxt;
    [SerializeField] private string consoleLog;
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Image deleteIcon;
    public int currentPoints;
    private Sprite _trashClosed, _trashOpen;

    private void Start() {
        _trashClosed = Resources.Load<Sprite>("Sprites/trash_closed");
        _trashOpen = Resources.Load<Sprite>("Sprites/trash_open");
    }

    public void SetTrashIcon(bool isOpen) {
        Debug.Log(_trashOpen);
        deleteIcon.sprite = isOpen ? _trashOpen : _trashClosed;
    }

    public void AddPoints(int point) {
        currentPoints += point;
    }

    public void ReducePoints(int point) {
        currentPoints -= point;
    }

    public void ResetBlocks() {
        currentPoints = 0;
    }

    // Recursion to check all the child of block parent 
    public void CheckConsoleValues(Transform parent)
    {
        // Loop through all child objects of the parent
        foreach (Transform child in parent)
        {
            // Check if the child has a BlockInput script
            BlockInput blockInput = child.GetComponent<BlockInput>();
            if (blockInput != null)
            {
                Debug.Log(child);
                // Check the console value of the BlockInput script
                if (blockInput.consoleValue != "")
                {
                    consoleLog += blockInput.consoleValue + "\n";
                }
            }
            
            // Check the child's descendants recursively
            CheckConsoleValues(child);
        }
    }

    public void RunCommands() {
        consoleLog = "";
        CheckConsoleValues(blocksParent);
        consoleTxt.text = consoleLog;
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredPoints == currentPoints) {
            Debug.Log(currentPoints);
        }
    }
}
