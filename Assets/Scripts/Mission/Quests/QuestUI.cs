using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [Header("Quest Infos")]
    [SerializeField] private TextMeshProUGUI[] questTitles;
    [SerializeField] private TextMeshProUGUI[] questDescriptions;
    [SerializeField] private TextMeshProUGUI[] questObjectives;

    [Header("Quest Side Panel")]
    [SerializeField] private GameObject s_questContentPrefab;
    [SerializeField] private GameObject s_questPanelPrefab;
    [SerializeField] private Transform s_questPanelParent;
    [SerializeField] private Transform s_questContentParent;

    [Header("Quest Log")]
    [SerializeField] private TextMeshProUGUI l_questRewardExp;
    [SerializeField] private TextMeshProUGUI l_questRewardMoney;
    [SerializeField] private GameObject l_questButtonPrefab;
    [SerializeField] private GameObject l_questContentPrefab;
    [SerializeField] private GameObject l_giveUpBtn;
    [SerializeField] private Transform l_mainQuestContentParent;
    [SerializeField] private Transform l_mainQuestButtonParent;
    [SerializeField] private Transform l_sideQuestParent;

    [Header("Quest PopUp")]
    [SerializeField] private GameObject p_questPrefab;
    [SerializeField] private TextMeshProUGUI p_missionTypeTxt;

    private QuestObject _currentQuestObject;

    [Header("Quest Lists")]
    public List<Quest> availableQuest = new List<Quest>();
    public List<Quest> activeQuest = new List<Quest>();

    [Header("Params")]
    [SerializeField] private Transform qButtonSpacer;


    public static QuestUI Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    // Side Panel UI
    public void SetSPQuestUI() {
        if (s_questPanelParent.childCount > 0) {
            Destroy(s_questPanelParent.GetChild(0).gameObject);
        }
        
        foreach (var item in activeQuest)
        {
            if (item.questType == Quest.QuestType.MAIN) {
                // Setup the side panel UI
                GameObject sidePanelQuest = Instantiate(s_questPanelPrefab, s_questPanelParent); // Create a new gameobject based on prefab
                QuestUISidePanel qUISidePanel = sidePanelQuest.GetComponent<QuestUISidePanel>(); // Call the script from prefab
                qUISidePanel.SetQuestTitle(item.title); // Set the quest title
                qUISidePanel.SetQuestSubTitle(item.description); // Set the quest description
                qUISidePanel.GenerateObjectives(item.objectives); // Generate the quest objectives
                
                foreach (var title in questTitles)
                {
                    title.text = item.title;
                }

                // Set popup text
                p_missionTypeTxt.text = item.title;

                sidePanelQuest.SetActive(true);

                // Logs
                if (l_mainQuestButtonParent.childCount > 0) Destroy(l_mainQuestButtonParent.GetChild(0).gameObject);

                GameObject questBtn = Instantiate(l_questButtonPrefab, l_mainQuestButtonParent);
                QuestButtonLog qBtnLog = questBtn.GetComponent<QuestButtonLog>();

                qBtnLog.questID = item.id;
                qBtnLog.questTitle.text = item.title;
            }
            
        }
        p_questPrefab.SetActive(true);
    }

    public void CheckQuest(QuestObject QO) {
    }


    // Side Panel Display of Current Main Quest
    public void AddSPMainQuest() {

    }
    
}
