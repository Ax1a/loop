using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public int id;
    public string subTitle;
    public string[] objectives;
    public string description;
    public string congratsText;
    public string summary;
    public int nextQuest;

    public string questObjective;
    public int questObjectiveCount;
    public int questObjectiveRequirement;

    public int expReward;
    public int moneyReward;

    public enum QuestProgress { NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE}
    public enum QuestType { MAIN, SIDE }
    public QuestProgress progress;
    public QuestType questType;
}
