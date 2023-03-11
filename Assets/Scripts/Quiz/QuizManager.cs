using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    #region Private Variables
    quizTimer timer;
    private QuestAndAns currentQuestion;
    int _totalQuestions = 0;
    int _questionIndex;
    int _energy;
    //bool isComplete = false;

    #endregion

    #region Public Variables
    public List<QuestAndAns> QnA;
    public GameObject[] options;
    public TextMeshProUGUI text;
    public GameObject quizPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    GameObject startPanel;
    public TextMeshProUGUI scoreTxt;
    public int scoreCount;
    public static QuizManager Instance;

    #endregion

#region Methods/Functions
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
        _totalQuestions = QnA.Count; 
        winPanel.SetActive(false);
        quizPanel.SetActive(true);
        timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        _energy = Energy.Instance.GetCurrentEnergy();    

    }
    public int GetScore()
    {
        return scoreCount;
    }

    public void StartGame () 
    {
        if (Energy.Instance.GetCurrentEnergy() > 0)
        {
            Energy.Instance.UseEnergy();
            quizTimer.Instance.startGame();
        }
        else
        {
            Debug.Log("No energy Left");
            return;
        }
    }
    public void Retry(){
        //to-do: check energy here
        if (Energy.Instance.GetCurrentEnergy() > 0)
        {
            ShuffleQuestions();
            _questionIndex = 0;
            scoreCount = 0;
            gameOverPanel.SetActive(false);
            timer.resetTime();
            SetCurrentQuestion(_questionIndex);
            Energy.Instance.UseEnergy();
        }
        else
        {
            Debug.Log("No energy Left");
            return;
        }
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
        gameOverPanel.SetActive(true);
        scoreTxt.text = scoreCount + "/" + _totalQuestions;
    }
    public void Win(){
        timer.stopTime();
        winPanel.SetActive(true);
    }
    public void OpenPanel ()
    {   
        if (scoreCount >= _totalQuestions)
        {
            Win();
            RewardManager.Instance.AssessReward();
        }
        else 
        {
            GameOver();
        }
    }
    public void Correct(){
        scoreCount +=1;
        SetCurrentQuestion(_questionIndex);
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

#endregion

}
