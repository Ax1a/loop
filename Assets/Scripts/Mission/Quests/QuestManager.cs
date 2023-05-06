using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private GameObject mainQuestGiver;
    public List<Quest> questList = new List<Quest>();
    public List<Quest> currentQuestList = new List<Quest>();

    public static QuestManager Instance;
    private bool firstLoad = true;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        if (DataManager.QuestList.Count == 0) {
            DataManager.QuestList = questList;
        }
        else {
            questList = DataManager.QuestList;
            currentQuestList = DataManager.CurrentQuests;
            QuestUI.Instance.activeQuest.AddRange(currentQuestList);
        }
        if (DataManager.GetTutorialProgress() >= 5) mainQuestGiver.SetActive(true);
    }

    public void QuestRequest(QuestObject QO) {
        // Available Quests
        if (QO.availableQuestIDs.Count > 0) {
            for (int i = 0; i < GetQuestCount(); i++) {
                for (int j = 0; j < QO.availableQuestIDs.Count; j++) {
                    // Check if the quest in the questList is available and matches the quest ID in QO.availableQuestIDs
                    if (questList[i].id == QO.availableQuestIDs[j] && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                        Debug.Log("Quest ID: " + QO.availableQuestIDs[j] + " " + questList[i].progress);

                        // Accept the quest and add it to the activeQuest list in the QuestUI
                        if (QO.isAutoAccept) {
                            AcceptQuest(QO.availableQuestIDs[j]);
                        }
                        else {
                            QuestUI.Instance.questAvailable = true;
                            QuestUI.Instance.availableQuest.Add(questList[i]);
                            Debug.Log("Added available quest" + questList[i].title);
                        }
                    }
                }
            }
        }

        // Active Quests
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            for (int j = 0; j < QO.receivableQuestIDs.Count; j++) {
                // Check if the quest in the currentQuestList is receivable and matches the quest ID in QO.receivableQuestIDs
                if (currentQuestList[i].id == QO.receivableQuestIDs[j] && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED || currentQuestList[i].progress == Quest.QuestProgress.COMPLETE) {
                    Debug.Log("Quest ID: " + QO.receivableQuestIDs[j] + " is " + currentQuestList[i].progress);

                    QuestUI.Instance.questRunning = true;
                    // if (!QuestUI.Instance.activeQuest.Contains(questList[i])) {
                    //     Debug.Log("Current List: " + currentQuestList[i].title);
                        // QuestUI.Instance.activeQuest.Add(questList[i]);
                        // Debug.Log("Added active quest" + questList[j].title);
                    // }
                    // CompleteQuest(QO.receivableQuestIDs[j]);
                }
            }
        }

        QuestUI.Instance.activeQuest.Clear();
        QuestUI.Instance.activeQuest.AddRange(currentQuestList);
        // Update the UI to show the available quests
        StartCoroutine(QuestUI.Instance.SetQuestUI());
        if (!firstLoad) {
            firstLoad = false;
            SaveGame.Instance.SaveGameState();
        }
    }

    // Accept quest based on the id 
    public void AcceptQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                currentQuestList.Add(questList[i]);
                Debug.Log("Accepted quest");
                // QuestUI.Instance.activeQuest.Add(questList[i]);
                questList[i].progress = Quest.QuestProgress.ACCEPTED;
                QuestUI.Instance.ShowNewQuestBanner(questList[i].title, questList[i].questType);
            }
        }
        
        DataManager.CurrentQuests = currentQuestList;
        QuestUI.Instance.activeQuest.Clear();
        QuestUI.Instance.activeQuest.AddRange(currentQuestList);
    }

    // The declined quest will not be available again.
    public void DeclineQuest(int questID) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].progress = Quest.QuestProgress.NOT_AVAILABLE;
                for (int j = 0; j < currentQuestList[i].questObjectiveCount.Length; j++)
                {
                    currentQuestList[i].questObjectiveCount[j] = 0;
                }
            }
        }

        DataManager.CurrentQuests = currentQuestList;
        QuestUI.Instance.activeQuest.Clear();
        QuestUI.Instance.activeQuest.AddRange(currentQuestList);
    }

    // Give up quest based on the id 
    public void GiveUpQuest(int questID) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].progress = Quest.QuestProgress.AVAILABLE;
                for (int j = 0; j < currentQuestList[i].questObjectiveCount.Length; j++)
                {
                    currentQuestList[i].questObjectiveCount[j] = 0;
                }
                currentQuestList.Remove(currentQuestList[i]);
            }
        }

        DataManager.CurrentQuests = currentQuestList;
    }

    public void EnableQuest(int questID) {
        for (int i = 0; i < questList.Count; i++) {
            if (questList[i].id == questID && questList[i].progress == Quest.QuestProgress.NOT_AVAILABLE) {
                questList[i].progress = Quest.QuestProgress.AVAILABLE;
                Debug.Log("test");
                // For testing
                foreach (var obj in GameObject.FindObjectsOfType<QuestObject>()) {
                    if (obj.gameObject.name == "[0] Main Quests") {
                        obj.gameObject.SetActive(false);
                        obj.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    // Complete quest based on the id 
    public void CompleteQuest(int questID) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.COMPLETE) {
                currentQuestList[i].progress = Quest.QuestProgress.DONE;
                // Give rewards
                DataManager.AddExp(currentQuestList[i].expReward);
                DataManager.AddMoney(currentQuestList[i].moneyReward);

                // Display the complete popup
                QuestUI.Instance.ShowCompleteQuestBanner(currentQuestList[i]);

                // Delete the completed mission from the list on UI
                for (int j = 0; j < QuestUI.Instance.activeQuest.Count; j++)
                {
                    if (QuestUI.Instance.activeQuest[j].id == currentQuestList[i].id) {
                        Debug.Log("Removed to UI:" + QuestUI.Instance.activeQuest[j].title);
                        QuestUI.Instance.activeQuest.Remove(QuestUI.Instance.activeQuest[j]);
                    }
                }
                currentQuestList.Remove(currentQuestList[i]);
                DataManager.CurrentQuests = currentQuestList;
                DataManager.QuestList = questList;
                // QuestUI.Instance.activeQuest = currentQuestList;
                QuestUI.Instance.ClearQuestData();
                // QuestUI.Instance.SetMainQuestUI();
            }
        }

        QuestUI.Instance.activeQuest.Clear();
        QuestUI.Instance.activeQuest.AddRange(currentQuestList);
        CheckChainQuest(questID);
        SaveGame.Instance.SaveGameState();
    }

    // This will check if the current mission has next mission
    // Can be used for the main quest
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

                    // For testing
                    foreach (var obj in GameObject.FindObjectsOfType<QuestObject>()) {
                        if (obj.gameObject.name == "[0] Main Quests") {
                            obj.gameObject.SetActive(false);
                            obj.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    // Add progress to the quest
    public void AddQuestItem(string questObjective, int itemAmount) {
        for (int i = 0; i < GetCurrentQuestCount(); i++) {
            Quest currentQuest = currentQuestList[i];

            // If the quest is not in the accepted state, skip it
            if (currentQuest.progress != Quest.QuestProgress.ACCEPTED) {
                continue;
            }
            
            // Get the index of the quest objective that matches the one given as a parameter
            int objectiveIndex = Array.IndexOf(currentQuest.questObjectives, questObjective);
            // If the given quest objective is not part of the current quest, skip it
            if (objectiveIndex == -1) {
                continue;
            }
            
            currentQuest.questObjectiveCount[objectiveIndex] += itemAmount;

            // If the count of the given quest objective is equal to or greater than the required amount,
            // set the count to the required amount and check if all quest objectives have been completed
            if (currentQuest.questObjectiveCount[objectiveIndex] >= currentQuest.questObjectiveRequirement[objectiveIndex]) {
                currentQuest.questObjectiveCount[objectiveIndex] = currentQuest.questObjectiveRequirement[objectiveIndex];
                if (currentQuest.questObjectiveCount.SequenceEqual(currentQuest.questObjectiveRequirement)) {
                    currentQuest.progress = Quest.QuestProgress.COMPLETE;

                    // Automatic claim the rewards
                    CompleteQuest(currentQuest.id);
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
