[System.Serializable]
public class InteractionQuiz
{
    public int quizID;
    public bool isComplete = false;
    public bool isActive = false;
    public string quizTitle;
    public string quizDescription;
    public enum quizLanguage { C, PYTHON, JAVA }
    public quizLanguage language;
}
