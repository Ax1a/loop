using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValidateController : MonoBehaviour
{
    #region Variables
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
    private bool errorDetected = false, blocksPlaced = false, coroutineRunning = false;
    #endregion
    
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
        Debug.Log("Added points");
        currentPoints += point;
    }

    public void ReducePoints(int point) {
        Debug.Log("Reduced Points");
        currentPoints -= point;
    }

    public void AskForInput(BlockVariable blockVariable) {
        _blockVariable = blockVariable;
        variableInputPanel.SetActive(true);
    }

    public void InsertInput(TMP_InputField input) {
        _blockVariable.SetDictionaryValue(input.text);
        _blockVariable._stringArray = _blockVariable.originalObj.GetComponent<BlockVariable>()._stringArray;
        _blockVariable._stringVar = _blockVariable.originalObj.GetComponent<BlockVariable>()._stringVar;
        _blockVariable._intArray = _blockVariable.originalObj.GetComponent<BlockVariable>()._intArray;
        _blockVariable._intVar = _blockVariable.originalObj.GetComponent<BlockVariable>()._intVar;
        _blockVariable.inputChanged = true;
        _blockVariable.originalObj.GetComponent<BlockDrag>().inputChanged = true;
        
        // _consoleLog += "\n" + input.text;
        variableInputPanel.SetActive(false);
        StartCoroutine(DelayDisable());
    }

    private IEnumerator DelayDisable() {
        yield return new WaitForSeconds(0.1f);
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
    public IEnumerator CheckBlocksPlaced(Transform parent)
    {
        errorDetected = false;
        // Loop through all child objects of the parent
        foreach (Transform child in parent)
        {
            if (child.name.Equals("IfCondition")) {
                if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "false") continue;
            }
            else if (child.name.Equals("ElseCondition")) {
                if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "true") continue;
            }

            BlockDrag blockDrag = child.GetComponent<BlockDrag>();
            if (blockDrag != null) {
                blocksPlaced = true;
                if (blockDrag.error) {
                    errorDetected = true;
                    break;
                }

                if (blockDrag.blockType == BlockDrag.BlockType.Type1) {
                    BlockOneDrop blockOneDrop = child.GetComponent<BlockOneDrop>();

                    if (child.name.StartsWith("C_IfCondition") && !blockDrag.consoleValue.ToLower().Equals("true"))
                    {
                        continue; // skip this block and its children
                    }
                    
                    
                    if (blockOneDrop != null && child.transform.name.StartsWith("C_CharInput")) {
                        BlockVariable _blockVariable = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                        BlockDrag _blockDrag = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();

                        if (_blockVariable != null) {
                            AskForInput(_blockVariable);
                            _inputPanelOpen = true;
                            yield return WaitForInputPanelToClose();
                        }
                        else if (_blockDrag != null && _blockDrag.consoleValue.Length != 0 && _blockDrag.printConsole) {
                            _blockDrag.consoleValue = "";
                        }
                    }
                    else if (blockDrag.consoleValue != "" && blockDrag.printConsole)
                    {
                        if (child.name.StartsWith("C_Print")) Debug.Log("updated print value");
                        _consoleLog += blockDrag.consoleValue + "\n";
                    }

                }   
                // blockDrag.inputChanged = true; 
            }
            if (errorDetected) break;

            StartCoroutine(CheckBlocksPlaced(child));
        }
    }

    public void ExecuteCommand() {
        if (coroutineRunning) return;
        StartCoroutine(RunCommands());
    }

    public IEnumerator RunCommands() {
        coroutineRunning = true;
        _consoleLog = "";
        consoleTxt.text = "";
        blocksPlaced = false;
        yield return StartCoroutine(CheckBlocksPlaced(blocksParent));
        coroutineRunning = false;

        if (!blocksPlaced) yield break;

        if (_inputPanelOpen) {
            yield return WaitForInputPanelToClose();
        }

        if (!errorDetected) {
            _consoleLog += "\n" + "...Program Finished";
        }
        else {
            _consoleLog = "\n" + "...Error";
        }
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

        if (_achievedPoints && !errorDetected) {
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

    private void OnDisable() {
        isStart = false;
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
        if (tempParent.childCount > 0) {
            foreach (Transform child in tempParent.transform)
            {
                BlockDrag blockDrag = child.GetComponent<BlockDrag>();

                if (blockDrag != null && blockDrag.addedPoints) {
                    blockDrag.addedPoints = false;
                    ReducePoints(1);
                }
            }
        }

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
