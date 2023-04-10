using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateController : MonoBehaviour
{
    [SerializeField] private int requiredPoints;
    public int currentPoints; // Hide
    [Header ("Objects")]
    [SerializeField] private TextMeshProUGUI consoleTxt;
    [SerializeField] private Image deleteIcon;
    
    [Header ("Blocks Parents")]
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform tempParent;
    private string _consoleLog;
    private Sprite _trashClosed, _trashOpen;

    private void Start() {
        _trashClosed = Resources.Load<Sprite>("Sprites/trash_closed");
        _trashOpen = Resources.Load<Sprite>("Sprites/trash_open");
    }

    public void SetTrashIcon(bool isOpen) {
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
        foreach (Transform child in blocksParent)
        {
            Image placeholder = child.GetComponent<Image>();
            Color color = placeholder.color;
            color.a = 0.5f;
            placeholder.color = color; 
            foreach (Transform subChild in child)
            {
                Destroy(subChild.gameObject);
            }
        }

        foreach (Transform child in tempParent)
        {
            Destroy(child.gameObject);
        }
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
                // Check the console value of the BlockInput script
                if (blockInput.consoleValue != "")
                {
                    _consoleLog += blockInput.consoleValue + "\n";
                }
            }
            
            // Check the child's descendants recursively
            CheckConsoleValues(child);
        }
    }

    public void RunCommands() {
        _consoleLog = "";
        CheckConsoleValues(blocksParent);
        _consoleLog += "...Program Finished";
        consoleTxt.text = _consoleLog;
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredPoints == currentPoints) {
            Debug.Log(currentPoints);
        }
    }
}
