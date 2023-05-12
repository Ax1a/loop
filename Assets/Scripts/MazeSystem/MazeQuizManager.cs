using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestionAndAnswer
{
    public string Questions;
    public string[] Answer;
    public int CorrectAns;
    public bool Answered;
}
public class MazeQuizManager : MonoBehaviour
{
    [HideInInspector] public int _questionIndex;
    [HideInInspector] public int pointsToWin;
    [HideInInspector] public int currentPoints;
    public GameObject QuizPanel;
    public QuestionAndAnswer QnA;
    public GameObject[] options;
    private QuestionAndAnswer currentQuestion;
    public TextMeshProUGUI text;
    void Start()
    {
        GenerateQuestion();
        currentPoints = 0;
        pointsToWin = 1;
        currentQuestion.Answered = false;
    }

    public void Correct()
    {
        // scoreCount += 1;
        GenerateQuestion();
        currentQuestion.Answered = true;
        currentPoints += 1;
        Debug.Log("Answer is correct");
    }
    public void Wrong()
    {
        currentQuestion.Answered = true;
        GenerateQuestion();
        Debug.Log("Answer is wrong");

    }
    private void GenerateQuestion()
    {
        if (currentQuestion != null)
        {
            currentQuestion.Answered = true;
        }

        currentQuestion = QnA;

        text.text = currentQuestion.Questions;

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerManager>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentQuestion.Answer[i];

            if (currentQuestion.CorrectAns == i + 1)
            {
                options[i].GetComponent<AnswerManager>().isCorrect = true;
            }
        }

    }
}
