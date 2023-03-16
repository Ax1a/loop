using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] private GameObject l_questContentPrefab;
    [SerializeField] private GameObject l_questExpContentPrefab;
    [SerializeField] private GameObject l_questMoneyContentPrefab;
    [SerializeField] private GameObject l_giveUpBtn;
    [SerializeField] private Transform l_mainQuestButtonParent;
    [SerializeField] private Transform l_mainQuestObjectiveParent;

    [Header("Quest PopUp")]
    // New Mission Popup
    [SerializeField] private GameObject p_questPrefab;
    [SerializeField] private TextMeshProUGUI p_missionTypeTxt;

    // For complete Popup
    [SerializeField] private GameObject p_questCompletePrefab;
    [SerializeField] private TextMeshProUGUI p_questCompleteTitle;
    [SerializeField] private TextMeshProUGUI p_questType;
    [SerializeField] private TextMeshProUGUI p_questRewardExp;
    [SerializeField] private TextMeshProUGUI p_questRewardMoney;

    private QuestObject _currentQuestObject;

    [Header("Quest Lists")]
    public List<Quest> availableQuest = new List<Quest>();
    public List<Quest> activeQuest = new List<Quest>();
    private List<GameObject> _questButtons = new List<GameObject>();

    public static QuestUI Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    // Set Up Main Quests
    public void SetMainQuestUI() {
        int sidePanelQCount = 0;

        if (s_questPanelParent.childCount > 0) {
            Destroy(s_questPanelParent.GetChild(0).gameObject);
        }
        
        foreach (var item in activeQuest)
        {
            if (item.questType == Quest.QuestType.MAIN) {
                if (sidePanelQCount == 0) {
                    ShowQuestSidePanel(item);
                    sidePanelQCount++;
                }

                // Set popup text
                p_missionTypeTxt.text = item.title;
                
                // Display the button list on quest log
                FillQuestLogButtons(item);
            }
            
        }

        // Show the popup when new mission appears
        p_questPrefab.SetActive(true);
    }

    // Fill up quest log buttons
    public void FillQuestLogButtons(Quest item) {
        if (l_mainQuestButtonParent.childCount > 0) Destroy(l_mainQuestButtonParent.GetChild(0).gameObject);

        GameObject questBtn = Instantiate(l_questButtonPrefab, l_mainQuestButtonParent);
        QuestButtonLog qBtnLog = questBtn.GetComponent<QuestButtonLog>();

        qBtnLog.questID = item.id;
        qBtnLog.questTitle.text = item.title;
        _questButtons.Add(questBtn);
    }

    public void ShowQuestSidePanel(Quest item) {
        // Setup the side panel UI
        GameObject sidePanelQuest = Instantiate(s_questPanelPrefab, s_questPanelParent); // Create a new gameobject based on prefab
        QuestUISidePanel qUISidePanel = sidePanelQuest.GetComponent<QuestUISidePanel>(); // Call the script from prefab
        qUISidePanel.SetQuestTitle(item.title); // Set the quest title
        qUISidePanel.SetQuestSubTitle(item.subTitle); // Set the quest description
        qUISidePanel.GenerateObjectives(item.questObjectives); // Generate the quest objectives
        
        sidePanelQuest.SetActive(true);
    }

    // Show selected log quest
    public void ShowSelectedQuest(int questID) {
        l_questExpContentPrefab.SetActive(false);
        l_questMoneyContentPrefab.SetActive(false);

        foreach (var quest in activeQuest)
        {
            if (quest.id == questID) {
                l_questTitle.text = quest.title;
                l_questDescription.text = quest.description;

                if (l_mainQuestObjectiveParent.childCount > 0) {
                    foreach (Transform child in l_mainQuestObjectiveParent)
                    {
                        Destroy(child.gameObject);
                    }
                }
                foreach (var objective in quest.questObjectives)
                {
                    GameObject _objective = Instantiate(l_questObjectivePrefab ,l_mainQuestObjectiveParent);
                    _objective.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = objective;
                }

                if (quest.expReward > 0) {
                    l_questRewardExp.text = "+" + quest.expReward.ToString();
                    l_questExpContentPrefab.SetActive(true);
                }

                if (quest.moneyReward > 0) {
                    l_questRewardMoney.text = "+" + quest.moneyReward.ToString();
                    l_questMoneyContentPrefab.SetActive(true);
                }
            }
        }
    }

    // Display when finished a mission
    public void ShowCompleteQuestBanner(Quest quest) {
        p_questCompleteTitle.text = quest.title;

        if (quest.expReward > 0) {
            p_questRewardExp.transform.parent.gameObject.SetActive(true);
            p_questRewardExp.text = "+ " + quest.expReward;
        }
        else {
            p_questRewardExp.transform.parent.gameObject.SetActive(false);
        }

        if (quest.moneyReward > 0) {
            p_questRewardMoney.transform.parent.gameObject.SetActive(true);
            p_questRewardMoney.text = "+ " + quest.moneyReward;
        }
        else {
            p_questRewardMoney.transform.parent.gameObject.SetActive(false);
        }

        if (quest.questType == Quest.QuestType.MAIN) {
            p_questType.text = "Main Quest";
        }
        else {
            p_questType.text = "Side Quest";
        }

        p_questCompletePrefab.SetActive(true);
    }

    // Display the first quest on load
    public void DisplayFirstQuest() {
        int _questID = l_mainQuestButtonParent.GetChild(0).gameObject.GetComponent<QuestButtonLog>().questID;

        ShowSelectedQuest(_questID);
    }

    // Check the quest list based on the parameter
    public void CheckQuest(QuestObject QO) {
        _currentQuestObject = QO;
        QuestManager.Instance.QuestRequest(QO);
    }

    // Clear Quest Data to avoid duplication
    public void ClearQuestData() {
        // availableQuest.Clear();
        // activeQuest.Clear();

        foreach (var item in _questButtons)
        {
            Destroy(item);
        }
        _questButtons.Clear();
    }
    
}
