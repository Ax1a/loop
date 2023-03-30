using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TextMeshProUGUI[] scoreTxt;
    public TextMeshProUGUI[] rewardTxt;
    [HideInInspector] public int scoreCount;
    public static QuizManager Instance;
    public GameObject[] rewardsPanel;

    #endregion

    #region Methods/Functions
    private void Awake()
    {
        if (Instance == null)
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

    public void StartGame()
    {
        if (Energy.Instance.GetCurrentEnergy() > 0)
        {
            Energy.Instance.UseEnergy(1);
            quizTimer.Instance.startGame();
        }
        else
        {
            Debug.Log("No energy Left");
            return;
        }
    }
    public void Retry()
    {
        ShuffleQuestions();
        //to-do: check energy here
        if (Energy.Instance.GetCurrentEnergy() > 0)
        {
            _questionIndex = 0;
            scoreCount = 0;
            gameOverPanel.SetActive(false);
            timer.resetTime();
            SetCurrentQuestion(_questionIndex);
            Energy.Instance.UseEnergy(1);
        }
        else
        {
            Debug.Log("No energy Left");
            return;
        }
    }
    private void ShuffleQuestions()
    {
        int Compare(QuestAndAns a, QuestAndAns b)
        {
            return Random.Range(-1, 2);
        }

        QnA.Sort(Compare);

        // Add questions
        SetCurrentQuestion(_questionIndex);
    }
    public void GameOver()
    {
        timer.stopTime();
        gameOverPanel.SetActive(true);
        //Lose score index : 0
        scoreTxt[0].text = scoreCount + "/" + _totalQuestions;
    }
    public void Win()
    {
        timer.stopTime();
        winPanel.SetActive(true);
        //Lose score index : 1
        scoreTxt[1].text = scoreCount + "/" + _totalQuestions;
        StartCoroutine(DisplayRewards());
    }
    public void OpenPanel()
    {
        if (scoreCount >= _totalQuestions)
        {
            Win();
            RewardManager.Instance.AssessReward();
            StartCoroutine(DelayAddProgress());
            // LessonsLevelManager.Instance.addReachedLesson();

            //to-do: should add level depending on the course they are taking
            DataManager.AddProgrammingLanguageProgress(LessonsLevelManager.Instance.course);
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator DelayAddProgress()
    {
        yield return new WaitForSeconds(3.5f);
        QuestManager.Instance.AddQuestItem("Finish lesson 1", 1);
    }
    public void Correct()
    {
        scoreCount += 1;
        SetCurrentQuestion(_questionIndex);
    }
    public void Wrong()
    {
        SetCurrentQuestion(_questionIndex);
    }
    void SetCurrentQuestion(int questionIndex)
    {
        if (questionIndex >= QnA.Count)
        {
            OpenPanel();
            return;
        };

        currentQuestion = QnA[questionIndex];

        text.text = currentQuestion.Questions;

        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentQuestion.Answer[i];

            if (currentQuestion.CorrectAns == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

        _questionIndex += 1;
    }

    IEnumerator DisplayRewards()
    {
        //Display Money Reward
        //Money index: 0 
        yield return new WaitForSeconds(1f);
        rewardTxt[0].text = "+" + RewardManager.Instance._money.ToString();
        rewardsPanel[0].SetActive(true);
        AudioManager.Instance.PlaySfx("Success");
        Debug.Log("Money: " + RewardManager.Instance._money);

        //Display Exp Reward
        //Exp index: 1
        yield return new WaitForSeconds(1f);
        rewardTxt[1].text = "+" + RewardManager.Instance._exp.ToString();
        rewardsPanel[1].SetActive(true);
        AudioManager.Instance.PlaySfx("Success");
        Debug.Log("Exp: " + RewardManager.Instance._exp);

        //Display button
        yield return new WaitForSeconds(1f);
        rewardsPanel[2].SetActive(true);

    }

    #endregion

}
