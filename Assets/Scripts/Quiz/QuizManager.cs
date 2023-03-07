using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestAndAns> QnA;
    private QuestAndAns currentQuestion;
    private int _questionIndex;
    public GameObject[] options;
    // public int currentQuestion;
    // public TextMesh Text;
    public TextMeshProUGUI text;
    public GameObject quizPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    GameObject startPanel;

    public TextMeshProUGUI scoreTxt;
    public int scoreCount;

    private quizTimer timer;
    int totalQuestions = 0;

    //bool isComplete = false;

    public static QuizManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ShuffleQuestions();
        totalQuestions = QnA.Count;
        // gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        quizPanel.SetActive(true);
        timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        
    }

    public int GetScore()
    {
        return scoreCount;
    }

    public void Retry(){
        ShuffleQuestions();
        _questionIndex = 0;
        scoreCount = 0;
        gameOverPanel.SetActive(false);
        // winPanel.SetActive(false);
        timer.resetTime();
        SetCurrentQuestion(_questionIndex);
    }

    private void ShuffleQuestions() {
        int Compare(QuestAndAns a, QuestAndAns b)
        {
            return Random.Range(-1, 2);
        }

        QnA.Sort(Compare);

        // Add questions
        SetCurrentQuestion(_questionIndex);
    }
    public void GameOver(){
        timer.stopTime();
        //quizPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        scoreTxt.text = scoreCount + "/" + totalQuestions;
      //  isComplete = true;
    }
    public void Win(){
        timer.stopTime();
        winPanel.SetActive(true);
        RewardManager.Instance.AssessReward();
    }
    public void OpenPanel ()
    {   
        if (scoreCount >= totalQuestions)
        {
            Win();
        }
        else 
        {
            GameOver();
        }
    }

    public void Correct(){
        scoreCount +=1;
        SetCurrentQuestion(_questionIndex);
        // QnA.RemoveAt(currentQuestion);
        // generateQuestion();
    }
    public void Wrong(){
        SetCurrentQuestion(_questionIndex);
    }
    void SetCurrentQuestion(int questionIndex){
        if (questionIndex >= QnA.Count) {
            OpenPanel();
            return;
        };

        currentQuestion = QnA[questionIndex];

        text.text = currentQuestion.Questions;

        for (int i =0; i<options.Length; i++){
            options[i].GetComponent<AnswerScript>().isCorrect= false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentQuestion.Answer[i];
       
            if(currentQuestion.CorrectAns == i+1){
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

        _questionIndex += 1;
    }

}
