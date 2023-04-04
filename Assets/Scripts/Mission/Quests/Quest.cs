[System.Serializable]
public class Quest
{
    public string title;
    public int id;
    public string subTitle;
    public string description;
    public string congratsText;
    public string npcName;
    public string summary;
    public int nextQuest;

    public string[] questObjectives;
    public int[] questObjectiveCount;
    public int[] questObjectiveRequirement;

    public int expReward;
    public int moneyReward;

    public enum QuestProgress { NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE}
    public enum QuestType { MAIN, SIDE }
    public QuestProgress progress;
    public QuestType questType;
}
