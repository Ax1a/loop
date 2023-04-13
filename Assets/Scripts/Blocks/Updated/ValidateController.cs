using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateController : MonoBehaviour
{
    [Header ("Params")]
    public int requiredPoints;
    public int moneyReward;
    public int expReward;
    private bool isStart;
    [SerializeField] private float timeLimit;
    public int currentPoints; // Hide
    private float currTime;

    [Header ("Objects")]
    [SerializeField] private TextMeshProUGUI consoleTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI moneyRewardTxt;
    [SerializeField] private TextMeshProUGUI expRewardTxt;
    [SerializeField] private Toggle taskCheck;
    [SerializeField] private Image deleteIcon;
    // [SerializeField] private GameObject variablePrefab;
    [Header ("Panels")]
    [SerializeField] private GameObject variableInputPanel;
    [SerializeField] private GameObject helpPopup;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject highlightGuide;
    
    [Header ("Blocks Parents")]
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform tempParent;

    [Header ("Scripts")]
    [SerializeField] private InteractionQuizInfo interactionQuiz;
    private string _consoleLog;
    private Sprite _trashClosed, _trashOpen;
    private BlockVariable _blockVariable;
    private bool _inputPanelOpen = false;
    private bool _achievedPoints = false;
    
    private void Start() {
        _trashClosed = Resources.Load<Sprite>("Sprites/trash_closed");
        _trashOpen = Resources.Load<Sprite>("Sprites/trash_open");
    }

    private void OnEnable() {
        // For testing
        // PlayerPrefs.DeleteKey("FirstInteract");

        startPanel.SetActive(true);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        _achievedPoints = false;
        currTime = timeLimit;
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

        if (blocksParent.childCount > 0) {
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
        }

        if (tempParent.childCount > 0) {
            foreach (Transform child in tempParent)
            {
                Destroy(child.gameObject);
            }
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

    public void SubmitBlocks() {
        isStart = false;

        if (_achievedPoints) {
            startPanel.SetActive(false);
            winPanel.SetActive(true);
            gameOverPanel.SetActive(false);

            if (!interactionQuiz.data.isComplete) {
                InteractionQuizManager.Instance.SetInteractionAsComplete(interactionQuiz);
                moneyRewardTxt.text = moneyReward.ToString();
                expRewardTxt.text = expReward.ToString();
                DataManager.AddExp(expReward);
                DataManager.AddMoney(moneyReward);
            }

        }
        else {
            startPanel.SetActive(false);
            winPanel.SetActive(false);
            gameOverPanel.SetActive(true);
        }
    }

    public void Retry() {
        currentPoints = 0;
        ResetBlocks();
        startPanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        isStart = true;
        currTime = timeLimit;
    }

    public void ToggleHelpPopup(bool isOpen) {
        if (!PlayerPrefs.HasKey("FirstInteract") && isOpen) {
            UIController.Instance.DequeuePopupHighlight(0);
        }
        else if (!PlayerPrefs.HasKey("FirstInteract") && !isOpen) {
            PlayerPrefs.SetInt("FirstInteract", 1);
            BotGuide.Instance.AddDialogue("Great job on learning the basics, now it's time to put your block-building skills to the test!");
            BotGuide.Instance.ShowDialogue();
        }
        isStart = !isOpen;
        helpPopup.SetActive(isOpen);
    }

    public void StartGame()
    {
        if (!isStart)
        {
            if (Energy.Instance.GetCurrentEnergy() > 0)
            {
                currentPoints = 0;
                currTime = timeLimit;
                Energy.Instance.UseEnergy(1);
                isStart = true;
                startPanel.SetActive(false);
                winPanel.SetActive(false);
                gameOverPanel.SetActive(false);

                if (!PlayerPrefs.HasKey("FirstInteract")) {
                    BotGuide.Instance.AddDialogue("Hello there! Let's put your block-building skills to the test and have some fun!");
                    BotGuide.Instance.AddDialogue("But first, let's cover the basics first before we dive into the interactive quiz.");
                    BotGuide.Instance.ShowDialogue();
                    UIController.Instance.EnqueuePopup(highlightGuide);
                }
            }
            else
            {
                NPCDialogue.Instance.AddDialogue("I'm exhausted and need to rest. Where can I go to recharge and regain some energy?", DataManager.GetPlayerName());
                NPCDialogue.Instance.ShowDialogue();
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPoints >= requiredPoints) {
            _achievedPoints = true;
        }
        else {
            _achievedPoints = false;
        }

        if (isStart)
        {
            currTime -= 1 * Time.deltaTime;
            float time = currTime;
            timerTxt.text = time.ToString("0");

            if (currTime <= 0)
            {
                currTime = 0;

                Debug.Log("Interactive Quiz: GameOver");

                startPanel.SetActive(false);
                winPanel.SetActive(false);
                gameOverPanel.SetActive(true);
                gameOverPanel.transform.SetAsLastSibling();
                isStart = false;
            }
        }
    }
}
