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
    [SerializeField] private GameObject inboxButtonPrefab;
    [SerializeField] private Transform inboxButtonParent;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    public static InboxManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
    
    private void OnEnable() {
        FillInboxButtons();
    }

    private void FillInboxButtons() {
        ClearData();

        // Add generation of buttons
        foreach (var item in DataManager.QuestList)
        {
            Debug.Log(item.questType + " " + item.progress + " " + item.npcName);
            if (item.questType == Quest.QuestType.SIDE && item.progress == Quest.QuestProgress.AVAILABLE && item.npcName != "") {
                GameObject _inboxItem = Instantiate(inboxButtonPrefab, inboxButtonParent);
                InboxButton _inboxButton = _inboxItem.GetComponent<InboxButton>();
                
                _inboxButton.SetTitle(item.title);
                _inboxButton.SetDescription(item.description);
                _inboxButton.SetID(item.id);
            }
        }
    }

    public void SetUpInboxDetails(int id) {
        foreach (var item in DataManager.QuestList)
        {
            if (item.id == id) {
                title.text = item.title;
                description.text = item.description;
                npcName.text = item.npcName;
                expReward.text = "+" + item.expReward;
                moneyReward.text = "+" + item.moneyReward;
            }
        }
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
