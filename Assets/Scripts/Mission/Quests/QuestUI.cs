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
    [SerializeField] private Transform l_mainQuestParent;
    [SerializeField] private Transform l_sideQuestParent;

    [Header("Quest PopUp")]
    [SerializeField] private GameObject p_questPrefab;
    [SerializeField] private TextMeshProUGUI p_missionTypeTxt;

    private QuestObject _currentQuestObject;
    
    [Header("Quest Buttons")]
    [SerializeField] private GameObject questBtn;
    [SerializeField] private GameObject acceptBtn;
    [SerializeField] private GameObject giveUpBtn;

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
                GameObject sidePanelQuest = Instantiate(s_questPanelPrefab, s_questPanelParent);
                sidePanelQuest.SetActive(true);
                p_missionTypeTxt.text = item.title;

                foreach (var title in questTitles)
                {
                    title.text = item.title;
                }

                foreach (var req in item.objectives)
                {
                    GameObject requirement = Instantiate(s_questContentPrefab, s_questContentParent.transform);    
                    requirement.GetComponentInChildren<TextMeshProUGUI>().text = req;    
                    // TextMeshProUGUI requirement = Instantiate(requirementText, parent.transform);    
                    // requirement.text = req;        
                    // requirement.transform.parent = parent.transform;
                }
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
