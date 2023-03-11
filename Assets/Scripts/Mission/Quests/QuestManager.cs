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
        DontDestroyOnLoad(gameObject);
    }

    // public void QuestRequest(QuestObject )

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
        for (int i = 0; i < GetQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].progress = Quest.QuestProgress.AVAILABLE;
                currentQuestList[i].questObjectiveCount = 0;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
    }

    // Complete quest
    public void CompleteQuest(int questID) {
        for (int i = 0; i < GetQuestCount(); i++) {
            if (currentQuestList[i].id == questID && currentQuestList[i].progress == Quest.QuestProgress.COMPLETE) {
                currentQuestList[i].progress = Quest.QuestProgress.DONE;
                currentQuestList.Remove(currentQuestList[i]);
            }
        }
    }

    // Add item to the list
    public void AddQuestItem(string questObjective, int itemAmount) {
        for (int i = 0; i < currentQuestList.Count; i++) {
            if (currentQuestList[i].questObjective == questObjective && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].questObjectiveCount += itemAmount;
            }

            if (currentQuestList[i].questObjectiveCount >= currentQuestList[i].questObjectiveRequirement && currentQuestList[i].progress == Quest.QuestProgress.ACCEPTED) {
                currentQuestList[i].progress = Quest.QuestProgress.COMPLETE;
           }
        }
    }


    // Remove item to the list
    

    /*
    * Bools for checking quest progress
    */

    public bool RequestAvailableQuest(int questID) {
        for (int i = 0; i < questList.Count; i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.AVAILABLE) {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int questID) {
        for (int i = 0; i < questList.Count; i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.ACCEPTED) {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompleteQuest(int questID) {
        for (int i = 0; i < questList.Count; i++) {
            if(questList[i].id == questID && questList[i].progress == Quest.QuestProgress.COMPLETE) {
                return true;
            }
        }
        return false;
    }

    public int GetQuestCount() {
        return questList.Count; 
    }
}
