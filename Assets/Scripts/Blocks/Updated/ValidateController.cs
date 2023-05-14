using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Block;

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
    [SerializeField] private GameObject loadingLog;
    
    [Header ("Blocks Parents")]
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform tempParent;

    [Header ("Scripts")]
    [SerializeField] private InteractionQuizInfo interactionQuiz;
    [HideInInspector] public string _consoleLog;
    [HideInInspector] public bool errorDetected = false, commandsRunning = false, resetBlocks = false;
    private Sprite _trashClosed, _trashOpen;
    private BlockVariable _blockVariable;
    private bool _achievedPoints = false;
    private bool processWait = false, blocksPlaced = false;
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

    public IEnumerator DelayDisable() {
        yield return new WaitForSeconds(0.1f);
        processWait = false;
    }

    public void ResetBlocks() {
        consoleTxt.text = "";
        processWait = false;
        resetBlocks = true;
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

    private IEnumerator UpdateBlocks(Transform parent, bool onRun) {
        foreach (Transform child in parent)
        {
            BlockDrag blockDrag = child.GetComponent<BlockDrag>();
            BlockVariable blockVariable = child.GetComponent<BlockVariable>();

            if (blockVariable != null && onRun) {
                blockVariable.declared = false;
                blockVariable.inputChanged = true;
            }
            else if (blockDrag != null) {
                blockDrag.inputChanged = true;
            }

            yield return UpdateBlocks(child, onRun);
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
                if (child.parent.GetComponent<BlockDrag>().error) {
                    errorDetected = true;
                    break;
                } 
                if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "false") continue;
            }
            else if (child.name.Equals("ElseCondition")) {
                if (child.parent.GetComponent<BlockDrag>().error) {
                    errorDetected = true;
                    break;
                } 
                if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "true") continue;
            }

            BlockDrag blockDrag = child.GetComponent<BlockDrag>();
            if (blockDrag != null) {
                blocksPlaced = true;

                // Execute loops
                BlockLoop blockLoop = child.GetComponent<BlockLoop>();
                BlockForLoop blockForLoop = child.GetComponent<BlockForLoop>();
                BlockForLoopP blockForLoopP = child.GetComponent<BlockForLoopP>();

                if (blockLoop != null) {
                    StartCoroutine(blockLoop.DelayLoop());
                    processWait = true;
                    yield return WaitForProcessToFinish();
                }
                else if (blockForLoop != null) {
                    StartCoroutine(blockForLoop.DelayLoop());
                    processWait = true;
                    yield return WaitForProcessToFinish();
                }   
                else if (blockForLoopP != null) {
                    StartCoroutine(blockForLoopP.DelayLoop());
                    processWait = true;
                    yield return WaitForProcessToFinish();
                }
                if (blockDrag.error) {
                    errorDetected = true;
                    break;
                }

                if (blockDrag.blockType == BlockDrag.BlockType.Type1) {
                    BlockOneDrop blockOneDrop = child.GetComponent<BlockOneDrop>();
                    BlockOperator blockOperator = child.GetComponent<BlockOperator>();

                    if (child.name.StartsWith("C_IfCondition") && !blockDrag.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("J_IfCondition") && !blockDrag.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("P_IfCondition") && !blockDrag.consoleValue.ToLower().Equals("true"))
                    {
                        continue; // skip this block and its children
                    }
                    else if (child.name.StartsWith("C_WhileLoop") || child.name.StartsWith("C_DoWhileLoop") || child.name.StartsWith("C_ForLoop") ||
                            child.name.StartsWith("J_WhileLoop") || child.name.StartsWith("J_DoWhileLoop") || child.name.StartsWith("J_ForLoop") ||
                            child.name.StartsWith("P_WhileLoop") || child.name.StartsWith("P_DoWhileLoop") || child.name.StartsWith("P_ForLoop")) {
                        continue;
                    }
                    
                    if (blockOneDrop != null && child.transform.name.StartsWith("C_CharInput")) {
                        BlockVariable _blockVariable = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                        BlockDrag _blockDrag = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();

                        if (_blockVariable != null) {
                            AskForInput(_blockVariable);
                            processWait = true;
                            yield return WaitForProcessToFinish();
                        }
                        else if (_blockDrag != null && _blockDrag.consoleValue.Length != 0 && _blockDrag.printConsole) {
                            _blockDrag.consoleValue = "";
                        }
                    }
                    else if (blockOperator != null) {
                        blockOperator.ExecuteAssignmentOperators();
                        blockOperator.DeclareVariable();
                        processWait = true;
                        yield return StartCoroutine(blockOperator.IncrementValue());
                        yield return StartCoroutine(UpdateBlocks(blocksParent, false));
                        StartCoroutine(DelayDisable());
                        yield return WaitForProcessToFinish();
                    }
                    else if (blockDrag.consoleValue != "" && blockDrag.printConsole)
                    {
                        _consoleLog += blockDrag.consoleValue + "\n";
                    }

                }   
            }
            if (errorDetected) break;

            StartCoroutine(CheckBlocksPlaced(child));
        }
    }

    public void ExecuteCommand() {
        if (CheckIfEnvironmentIsEmpty()) return;
        if (commandsRunning) return;
        StartCoroutine(RunCommands());
    }

    private bool CheckIfEnvironmentIsEmpty() {
        if (blocksParent.childCount > 0) {
            foreach (Transform child in blocksParent)
            {
                if (child.transform.childCount > 0) {
                    return false;
                }
            }
        }
        return true;
    }

    public IEnumerator RunCommands() {
        resetBlocks = true;
        _consoleLog = "";
        consoleTxt.text = "";
        loadingLog.SetActive(true);
        commandsRunning = true;
        yield return StartCoroutine(UpdateBlocks(blocksParent, true));
        blocksPlaced = false;
        yield return StartCoroutine(CheckBlocksPlaced(blocksParent));

        if (!blocksPlaced) yield break;

        if (processWait) {
            yield return WaitForProcessToFinish();
        }

        commandsRunning = false;
        loadingLog.SetActive(false);

        if (!errorDetected) {
            _consoleLog += "\n" + "...Program Finished";
        }
        else {
            _consoleLog = "\n" + "...Error";
        }
        consoleTxt.text = _consoleLog;
    }

    private IEnumerator WaitForProcessToFinish() {
        while (processWait) {
            yield return null;
        }
    }

    public void CancelExecute() {
        loadingLog.SetActive(false);
        consoleTxt.text = "";
        variableInputPanel.SetActive(false);
        errorDetected = true;
        processWait = false;
    }

    public void SubmitBlocks() {
        isStart = false;

        if (_achievedPoints && !errorDetected) {
            AudioManager.Instance.PlaySfx("Success");
            startPanel.SetActive(false);
            winPanel.SetActive(true);
            gameOverPanel.SetActive(false);
            // QuestManager.Instance.AddQuestItem("Open assignment on computer", 1);
            // QuestManager.Instance.AddQuestItem("Answer the assignment", 1);
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
            // if (Energy.Instance.GetCurrentEnergy() > 0)
            // {
                currentPoints = 0;
                currTime = timeLimit;
                // Energy.Instance.UseEnergy(1);
                isStart = true;
                startPanel.SetActive(false);
                winPanel.SetActive(false);
                gameOverPanel.SetActive(false);
                ResetBlocks();

                if (!PlayerPrefs.HasKey("FirstInteract")) {
                    BotGuide.Instance.AddDialogue("Hello there! Let's put your block-building skills to the test and have some fun!");
                    BotGuide.Instance.AddDialogue("But first, let's cover the basics first before we dive into the interactive quiz.");
                    BotGuide.Instance.ShowDialogue();
                    
                    if (highlightGuide != null)
                        UIController.Instance.EnqueuePopup(highlightGuide);
                }
            // }
            // else
            // {
            //     NPCDialogue.Instance.AddDialogue("I'm exhausted and need to rest. Where can I go to recharge and regain some energy?", DataManager.GetPlayerName());
            //     NPCDialogue.Instance.ShowDialogue();
            //     return;
            // }
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
