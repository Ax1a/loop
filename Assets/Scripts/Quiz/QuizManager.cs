using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{

    public List<QuestAndAns> QnA;
    public GameObject[] options;
    public int currentQuestion;
    // public TextMesh Text;
    public TextMeshProUGUI text;

    public GameObject quizPanel;
    public GameObject mainPanel;

    public TextMeshProUGUI scoreTxt;
    public int scoreCount;

    int totalQuestions = 0;

    // Start is called before the first frame update
    void Start()
    {
        generateQuestion();
        totalQuestions = QnA.Count;
        mainPanel.SetActive(false);
        
    }

    public void retry(){
        Debug.Log("Retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void gameOver(){
        quizPanel.SetActive(false);
        mainPanel.SetActive(true);
        scoreTxt.text = scoreCount + "/" + totalQuestions;
    }

    public void correct(){
        scoreCount +=1;
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
        
    }

    public void wrong(){
        QnA.RemoveAt(currentQuestion);
        generateQuestion();
    }
    void setAnswer(){
        for (int i =0; i<options.Length; i++){
            options[i].GetComponent<AnswerScript>().isCorrect= false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = QnA[currentQuestion].Answer[i];
       
            if(QnA[currentQuestion].CorrectAns == i+1){
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
        
    }

    void generateQuestion()
    {
        if(QnA.Count > 0){
            currentQuestion = Random.Range(0, QnA.Count);
            text.text = QnA[currentQuestion].Questions;
            setAnswer();
        }else{
            Debug.Log("Out of Questions");
            gameOver();
        }
       
        
    }

    
}
