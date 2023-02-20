using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{

    public List<QuestAndAns> QnA;
    public List<QuestAndAns> QnAHolder;
    private QuestAndAns currentQuestion;
    private int _questionIndex;
    public GameObject[] options;
    // public int currentQuestion;
    // public TextMesh Text;
    public TextMeshProUGUI text;
    public GameObject quizPanel;
    public GameObject gameOverPanel;

    public GameObject startPanel;

    public TextMeshProUGUI scoreTxt;
    public int scoreCount;

    public quizTimer timer;
    int totalQuestions = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShuffleQuestions();
        totalQuestions = QnA.Count;
        // gameOverPanel.SetActive(false);
        quizPanel.SetActive(true);
        timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        
    }
    public void Retry(){
        // Debug.Log("Retry");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ShuffleQuestions();
        _questionIndex = 0;
        scoreCount = 0;
        gameOverPanel.SetActive(false);
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
    }

    public void Correct(){
        scoreCount +=1;
        SetCurrentQuestion(_questionIndex);
        // QnA.RemoveAt(currentQuestion);
        // generateQuestion();
        
    }

    public void Wrong(){
        // QnA.RemoveAt(currentQuestion);
        // generateQuestion();
        SetCurrentQuestion(_questionIndex);
    }

    void SetCurrentQuestion(int questionIndex){
        if (questionIndex >= QnA.Count) {
            GameOver();
            return;
        };

        currentQuestion = QnA[questionIndex];

        Debug.Log(currentQuestion.Questions);

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

    void GenerateQuestion()
    {
        // if(QnA.Count > 0){
        //     currentQuestion = Random.Range(0, QnA.Count);
        //     text.text = QnA[currentQuestion].Questions;
        //     setAnswer();
        // }else{
        //     Debug.Log("Out of Questions");
        //     gameOver();
        // }
    }

    
}
