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
    // [SerializeField] private GameObject variablePrefab;
    [SerializeField] private GameObject variableInputPanel;
    
    [Header ("Blocks Parents")]
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform tempParent;
    private string _consoleLog;
    private Sprite _trashClosed, _trashOpen;
    private BlockVariable _blockVariable;
    private bool _inputPanelOpen = false;
    
    private void Start() {
        _trashClosed = Resources.Load<Sprite>("Sprites/trash_closed");
        _trashOpen = Resources.Load<Sprite>("Sprites/trash_open");

        // Generate variable blocks
        
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

    public void AskForInput(BlockVariable blockVariable) {
        _blockVariable = blockVariable;
        variableInputPanel.SetActive(true);
        _inputPanelOpen = true;
    }

    public void InsertInput(TMP_InputField input) {
        _blockVariable.SetDictionaryValue(input.text);
        _blockVariable._stringArray = _blockVariable.originalObj.GetComponent<BlockVariable>()._stringArray;
        _blockVariable._stringVar = _blockVariable.originalObj.GetComponent<BlockVariable>()._stringVar;
        _blockVariable._intArray = _blockVariable.originalObj.GetComponent<BlockVariable>()._intArray;
        _blockVariable._intVar = _blockVariable.originalObj.GetComponent<BlockVariable>()._intVar;
        _blockVariable.inputChanged = true;
        _blockVariable.originalObj.GetComponent<BlockVariable>().inputChanged = true;
        
        variableInputPanel.SetActive(false);
        _consoleLog += "\n" + input.text;
        _inputPanelOpen = false;
    }

    public void ResetBlocks() {
        consoleTxt.text = "";
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
    public void CheckBlocksPlaced(Transform parent)
    {
        // Loop through all child objects of the parent
        foreach (Transform child in parent)
        {
            // if (child.transform.childCount > 0) {
            BlockDrag blockDrag = child.GetComponent<BlockDrag>();
            if (blockDrag != null) {
                if (blockDrag.blockType == BlockDrag.BlockType.Type1) {
                    // Check if the child has a BlockInput script
                    BlockInput blockInput = child.GetComponent<BlockInput>();
                    BlockVariable blockVariable = child.GetComponent<BlockVariable>();
                    BlockOneDrop blockOneDrop = child.GetComponent<BlockOneDrop>();

                    if (blockInput != null)
                    {
                        // Check the console value of the BlockInput script
                        if (blockInput.consoleValue != "")
                        {
                            _consoleLog += blockInput.consoleValue + "\n";
                        }
                    }
                    else if (blockVariable != null) {
                        // Check the console value of the BlockVariable script
                        if (blockVariable.consoleValue != "")
                        {
                            _consoleLog += blockVariable.consoleValue + "\n";
                        }
                    }
                    else if (blockOneDrop != null && child.transform.name.StartsWith("CharInput")) {
                        BlockVariable _blockVariable = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                        BlockInput _blockInput = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockInput>();

                        if (_blockVariable != null) {
                            AskForInput(_blockVariable);
                        }
                        else if (_blockInput != null && _blockInput.consoleValue.Length != 0) {
                            _blockInput.consoleValue = "";
                        }
                    }
                    else if (blockOneDrop != null) {
                        if (blockOneDrop.consoleValue != "")
                        {
                            _consoleLog += blockOneDrop.consoleValue + "\n";
                        }
                    }
                }    
            }

            CheckBlocksPlaced(child);
        }
    }

    public void ExecuteCommand() {
        StartCoroutine(RunCommands());
    }

    public IEnumerator RunCommands() {
        _consoleLog = "";
        consoleTxt.text = "";
        CheckBlocksPlaced(blocksParent);
        yield return WaitForInputPanelToClose();
        _consoleLog += "\n" + "...Program Finished";
        consoleTxt.text = _consoleLog;
    }

    private IEnumerator WaitForInputPanelToClose() {
        while (_inputPanelOpen) {
            yield return null;
        }
    }

    public void CancelExecute() {
        consoleTxt.text = "";
        variableInputPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (requiredPoints == currentPoints) {
            Debug.Log(currentPoints);
        }
    }
}
