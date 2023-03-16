using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<Quest> questList = new List<Quest>();
    public List<Quest> currentQuestList = new List<Quest>();

    public static QuestManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    public void QuestRequest(QuestObject QO) {
        // Available Quests
        if (QO.availableQuestIDs.Count > 0) {
            for (int i = 0; i < GetQuestCount(); i++) {
                for (int j = 0; j < QO.availableQuestIDs.Count; j++) {
                    if (questList[i].id == QO.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                        Debug.Log("Quest ID: " + QO.availableQuestIDs[j] + " " + questList[i].progress);

                        // Testing
                        AcceptQuest(QO.availableQuestIDs[j]);
                        QuestUI.Instance.activeQuest.Add(questList[i]);
                    }
                }
            }
            QuestUI.Instance.SetMainQuestUI();
        }

        // Active Quests
        for (int i = 0; i > GetCurrentQuestCount(); i++) {
            for (int j = 0; j < QO.availableQuestIDs.Count; j++) {
                if (currentQuestList[i].id == QO.receivableQuestIDs[j] && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED || currentQuestList[i].progress == Quest.QuestProgress.COMPLETE) {
                    Debug.Log("Quest ID: " + QO.receivableQuestIDs[j] + " is " + currentQuestList[i].progress);

                    CompleteQuest(QO.receivableQuestIDs[j]);
                }
            }
        }
    }

    // Accept quest
    public void AcceptQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                currentQuestList.Add(questList[i]);
                questList[i].progress = Quest.QuestProgress.ACCEPTED;
            }
        }
    }

    // Give up quest
    public void GiveUpQuest(int questID) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].progress = Quest.QuestProgress.AVAILABLE;
                currentQuestList[i].questObjectiveCount = 0;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
    }

    // Complete quest
    public void CompleteQuest(int questID) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.COMPLETE) {
                currentQuestList[i].progress = Quest.QuestProgress.DONE;
                // Give rewards
                DataManager.AddExp(currentQuestList[i].expReward);
                DataManager.AddMoney(currentQuestList[i].moneyReward);

                // Display the complete popup
                QuestUI.Instance.ShowCompleteQuestBanner(currentQuestList[i]);

                currentQuestList.Remove(currentQuestList[i]);
                QuestUI.Instance.ClearQuestData();
            }
        }
        CheckChainQuest(questID);
    }

    void CheckChainQuest(int questID) {
        int tempID = 0;

        for (int i = 0; i < questList.Count; i++) {
            if (questList[i].id == questID && questList[i].nextQuest > 0) {
                tempID = questList[i].nextQuest;
            }
        }

        if (tempID > 0) {
            for (int i = 0; i < questList.Count; i++) {
                if (questList[i].id == tempID && questList[i].progress == Quest.QuestProgress.NOT_AVAILABLE) {
                    questList[i].progress = Quest.QuestProgress.AVAILABLE;
                }
            }
        }
    }

    // Add progress to the quest
    public void AddQuestItem(string questObjective, int itemAmount) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            foreach (var objective in currentQuestList[i].questObjectives)
            {
                if (objective == questObjective && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                    currentQuestList[i].questObjectiveCount += itemAmount;
                    Debug.Log(objective + questObjective);
                }

                if (currentQuestList[i].questObjectiveCount >= currentQuestList[i].questObjectiveRequirement && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                    currentQuestList[i].progress = Quest.QuestProgress.COMPLETE;

                    // Automatic claim the rewards
                    CompleteQuest(currentQuestList[i].id);
                }
            }
        }
    }

    /*
    * Bools for checking quest progress
    */

    public bool RequestAvailableQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.ACCEPTED) {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompleteQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.COMPLETE) {
                return true;
            }
        }
        return false;
    }

    public bool CheckAvailableQuest(QuestObject QO) {
        for (int i = 0; i < GetQuestCount(); i++) {
            for (int j = 0; j < QO.availableQuestIDs.Count; j++) {
                if (questList[i].id == QO.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckAcceptedQuest(QuestObject QO) {
        for (int i = 0; i < GetQuestCount(); i++) {
            for (int j = 0; j < QO.receivableQuestIDs.Count; j++) {
                if (questList[i].id == QO.receivableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.ACCEPTED) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckCompleteQuest(QuestObject QO) {
        for (int i = 0; i < GetQuestCount(); i++) {
            for (int j = 0; j < QO.receivableQuestIDs.Count; j++) {
                if (questList[i].id == QO.receivableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.COMPLETE) {
                    return true;
                }
            }
        }
        return false;
    }

    // Getters

    public int GetQuestCount() {
        return questList.Count; 
    }

    public int GetCurrentQuestCount() {
        return currentQuestList.Count; 
    }
}
