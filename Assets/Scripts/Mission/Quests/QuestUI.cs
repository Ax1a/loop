using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class QuestUI : MonoBehaviour
{
    [Header("Quest Side Panel")]
    [SerializeField] private GameObject s_questContentPrefab;
    [SerializeField] private GameObject s_questPanelPrefab;
    [SerializeField] private Transform s_questPanelParent;
    [SerializeField] private Transform s_questContentParent;

    [Header("Quest Log")]
    [SerializeField] private TextMeshProUGUI l_questRewardExp;
    [SerializeField] private TextMeshProUGUI l_questRewardMoney;
    [SerializeField] private TextMeshProUGUI l_questTitle;
    [SerializeField] private TextMeshProUGUI l_questDescription;
    [SerializeField] private GameObject l_questButtonPrefab;
    [SerializeField] private GameObject l_questObjectivePrefab;
    [SerializeField] private GameObject l_questExpContentPrefab;
    [SerializeField] private GameObject l_questMoneyContentPrefab;
    [SerializeField] private GameObject l_giveUpBtn;
    [SerializeField] private GameObject l_tabHighlight;
    [SerializeField] private GameObject l_questCanvas;
    [SerializeField] private Transform l_questButtonParent;
    [SerializeField] private Transform l_questObjectiveParent;
    private QuestButtonLog _firstQuestButton;

    [Header("Quest PopUp")]
    // New Mission Popup
    [SerializeField] private GameObject p_questPrefab;
    [SerializeField] private TextMeshProUGUI p_missionTypeTxt;
    [SerializeField] private TextMeshProUGUI p_questNewType;

    // For complete Popup
    [SerializeField] private GameObject p_questCompletePrefab;
    [SerializeField] private TextMeshProUGUI p_questCompleteTitle;
    [SerializeField] private TextMeshProUGUI p_questCompleteType;
    [SerializeField] private TextMeshProUGUI p_questRewardExp;
    [SerializeField] private TextMeshProUGUI p_questRewardMoney;

    private QuestObject _currentQuestObject;

    [Header("Quest Lists")]
    public List<Quest> availableQuest = new List<Quest>();
    public List<Quest> activeQuest = new List<Quest>();
    public List<GameObject> questButtons = new List<GameObject>();

    [Header("Quest UI Bools")]
    public bool questAvailable = false;
    public bool questRunning = false;
    private bool _questPanelActive = false;
    private bool _onMainQTab = false;
    public static QuestUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputManager.Instance.quest) && !UIController.Instance.OtherPanelActive())
        {
            _onMainQTab = true;
            l_tabHighlight.transform.DOLocalMoveX(-721.04f, .01f);
            UpdateQuestUI();
        }

        if (_questPanelActive != l_questCanvas.activeSelf) _questPanelActive = l_questCanvas.activeSelf;
    }

    // Set Up Main Quests
    public IEnumerator SetQuestUI()
    {
        if (s_questPanelParent.childCount > 0)
        {
            foreach (Transform child in s_questPanelParent)
            {
                Destroy(child.gameObject);
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var item in activeQuest)
        {
            if (item.questType == Quest.QuestType.MAIN)
            {   
                if (s_questPanelParent.childCount == 0)
                {
                    ShowQuestSidePanel(item);
                    break;
                }

            }
        }

        // Display the button list on quest log
        FillQuestLogButtons();
        // Show the popup when new mission appears
        // if (_questPanelActive == false) ShowNewQuestBanner(tempTitle, tempType);
    }

    public void ShowNewQuestBanner(string title, Quest.QuestType type)
    {
        p_missionTypeTxt.text = title;
        p_questNewType.text = type == Quest.QuestType.MAIN ? "Main Quest" : "Side Quest"; 
        UIController.Instance.EnqueuePopup(p_questPrefab);
        //Play SoundFx
        AudioManager.Instance.PlaySfx("New Mission");
    }

    // Fill up quest log buttons
    public void FillQuestLogButtons()
    {
        int _ctr = 0;

        questButtons.Clear();

        if (l_questButtonParent.childCount > 0)
        {
            foreach (Transform button in l_questButtonParent)
            {
                Destroy(button.gameObject);
            }
        }

        if (_onMainQTab) {
            foreach (var item in activeQuest)
            {
                if (item.questType == Quest.QuestType.MAIN) {
                    GameObject questBtn = Instantiate(l_questButtonPrefab, l_questButtonParent);
                    QuestButtonLog qBtnLog = questBtn.GetComponent<QuestButtonLog>();

                    if (_ctr == 0) _firstQuestButton = qBtnLog;
                    _ctr++;

                    qBtnLog.questID = item.id;
                    qBtnLog.questTitle.text = item.title;
                    questButtons.Add(questBtn);
                }
            }
        }
        else {
            foreach (var item in activeQuest)
            {
                if (item.questType == Quest.QuestType.SIDE) {
                    GameObject questBtn = Instantiate(l_questButtonPrefab, l_questButtonParent);
                    QuestButtonLog qBtnLog = questBtn.GetComponent<QuestButtonLog>();

                    if (_ctr == 0) _firstQuestButton = qBtnLog;
                    _ctr++;

                    qBtnLog.questID = item.id;
                    qBtnLog.questTitle.text = item.title;
                    questButtons.Add(questBtn);
                }
            }
        }

        DisplayFirstQuest();
    }

    public void SelectTab(string tab) {
        if (tab == "MAIN") {
            _onMainQTab = true;
            _firstQuestButton = null;
            FillQuestLogButtons();
            l_tabHighlight.transform.DOLocalMoveX(-721.04f, .35f).SetEase(Ease.OutSine);
        }
        else {
            _onMainQTab = false;
            _firstQuestButton = null;
            FillQuestLogButtons();
            l_tabHighlight.transform.DOLocalMoveX(-291.2f, .35f).SetEase(Ease.OutSine);
        }
    }

    public void ShowQuestSidePanel(Quest item)
    {
        // Setup the side panel UI
        GameObject sidePanelQuest = Instantiate(s_questPanelPrefab, s_questPanelParent); // Create a new gameobject based on prefab
        QuestUISidePanel qUISidePanel = sidePanelQuest.GetComponent<QuestUISidePanel>(); // Call the script from prefab
        qUISidePanel.SetQuestTitle(item.title); // Set the quest title
        qUISidePanel.SetQuestSubTitle(item.subTitle); // Set the quest description
        qUISidePanel.GenerateObjectives(item.questObjectives); // Generate the quest objectives

        sidePanelQuest.SetActive(true);
    }

    // Show selected log quest
    public void ShowSelectedQuest(int questID)
    {
        l_questExpContentPrefab.SetActive(false);
        l_questMoneyContentPrefab.SetActive(false);

        foreach (var quest in activeQuest)
        {
            if (quest.id == questID)
            {
                l_questTitle.text = quest.title;
                l_questDescription.text = quest.description;

                if (l_questObjectiveParent.childCount > 0)
                {
                    foreach (Transform child in l_questObjectiveParent)
                    {
                        Destroy(child.gameObject);
                    }
                }
                foreach (var objective in quest.questObjectives)
                {
                    GameObject _objective = Instantiate(l_questObjectivePrefab, l_questObjectiveParent);
                    _objective.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = objective;
                }

                if (quest.expReward > 0)
                {
                    l_questRewardExp.text = "+" + quest.expReward.ToString();
                    l_questExpContentPrefab.SetActive(true);
                }

                if (quest.moneyReward > 0)
                {
                    l_questRewardMoney.text = "+" + quest.moneyReward.ToString();
                    l_questMoneyContentPrefab.SetActive(true);
                }
            }
        }
    }

    // Display when finished a mission
    public void ShowCompleteQuestBanner(Quest quest)
    {
        p_questCompleteTitle.text = quest.title;

        if (quest.expReward > 0)
        {
            p_questRewardExp.transform.parent.gameObject.SetActive(true);
            p_questRewardExp.text = "+ " + quest.expReward;
        }
        else
        {
            p_questRewardExp.transform.parent.gameObject.SetActive(false);
        }

        if (quest.moneyReward > 0)
        {
            p_questRewardMoney.transform.parent.gameObject.SetActive(true);
            p_questRewardMoney.text = "+ " + quest.moneyReward;
        }
        else
        {
            p_questRewardMoney.transform.parent.gameObject.SetActive(false);
        }

        if (quest.questType == Quest.QuestType.MAIN)
        {
            p_questCompleteType.text = "Main Quest";
        }
        else
        {
            p_questCompleteType.text = "Side Quest";
        }

        // p_questCompletePrefab.SetActive(true);
        UIController.Instance.EnqueuePopup(p_questCompletePrefab);

        //Play SoundFx
        AudioManager.Instance.PlaySfx("Mission Complete");
    }

    // Display the first quest on load
    public void DisplayFirstQuest()
    {
        if (_firstQuestButton != null)
        {
            // QuestButtonLog _qButtonLog = l_questButtonParent.GetChild(0).gameObject.GetComponent<QuestButtonLog>();
            _firstQuestButton.ShowAllInfos();
        }
        else {
            ClearQuestUI();
        }
    }

    // Check the quest list based on the parameter
    public void CheckQuest(QuestObject QO)
    {
        _currentQuestObject = QO;
        QuestManager.Instance.QuestRequest(QO);

        if ((questRunning || questAvailable) && !_questPanelActive)
        {
            // UpdateQuestUI();
        }
        else
        {
            Debug.Log("No quest available");
        }
    }

    public void UpdateQuestUI()
    {
        _questPanelActive = true;
        ClearQuestData();
        StartCoroutine(SetQuestUI());
    }

    // Clear Quest Data to avoid duplication
    public void ClearQuestData()
    {
        ClearQuestUI();

        if (s_questPanelParent.childCount > 0)
        {
            Destroy(s_questPanelParent.GetChild(0).gameObject);
        }

        // Remove the quest of the buttons that does not exist
        foreach (var item in questButtons)
        {
            Destroy(item);
        }

        questButtons.Clear();
    }

    public void ClearQuestUI() {
        l_questTitle.text = "No quest available.";
        l_questDescription.text = "";
        l_questRewardExp.text = "";
        l_questRewardMoney.text = "";
        l_questExpContentPrefab.SetActive(false);
        l_questMoneyContentPrefab.SetActive(false);

        // Remove the old objectives
        if (l_questObjectiveParent.childCount > 0)
        {
            foreach (Transform child in l_questObjectiveParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
