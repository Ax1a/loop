using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InboxManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI expReward;
    [SerializeField] private TextMeshProUGUI moneyReward;
    [SerializeField] private GameObject emptyContainer;
    [SerializeField] private GameObject detailsContainer;
    [SerializeField] private GameObject inboxButtonPrefab;
    [SerializeField] private Transform inboxButtonParent;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    private InboxButton _firstInbox;
    public static InboxManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
    
    private void OnEnable() {
        FillInboxButtons();
    }

    public void FillInboxButtons() {
        ClearData();
        int count = 0;

        // Add generation of buttons
        foreach (var item in DataManager.QuestList)
        {
            if (item.questType == Quest.QuestType.SIDE && item.progress == Quest.QuestProgress.AVAILABLE && item.npcName != "") {
                GameObject _inboxItem = Instantiate(inboxButtonPrefab, inboxButtonParent);
                InboxButton _inboxButton = _inboxItem.GetComponent<InboxButton>();
                
                if (_firstInbox == null) _firstInbox = _inboxButton;

                _inboxButton.SetTitle(item.title);
                _inboxButton.SetDescription(item.description);
                _inboxButton.SetID(item.id);
                count++;
            }
        }

        if (count > 0) {
            emptyContainer.SetActive(false);
            detailsContainer.SetActive(true);
            SetUpInboxDetails(_firstInbox.id);
        }
        else {
            emptyContainer.SetActive(true);
            detailsContainer.SetActive(false);
        }
    }

    public void SetUpInboxDetails(int id) {
        foreach (var item in DataManager.QuestList)
        {
            if (item.id == id) {
                title.text = item.title;
                description.text = item.description;
                npcName.text = "- " + item.npcName;
                expReward.text = "+" + item.expReward;
                moneyReward.text = "+" + item.moneyReward;

                acceptButton.onClick.RemoveAllListeners();
                acceptButton.onClick.AddListener(() => AcceptSideQuest(item));
            }
        }
    }

    private void AcceptSideQuest(Quest quest) {
        QuestManager.Instance.AcceptQuest(quest.id);
        QuestUI.Instance.ShowNewQuestBanner(quest.title, quest.questType);
    }

    private void ClearData() {
        // Clear old buttons
        if (inboxButtonParent.childCount > 0) {
            foreach (Transform child in inboxButtonParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
