using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    #region Private Variables
    private QuestAndAns currentQuestion;
    int _totalQuestions = 0;
    int _energy;
    private string _currentCourse;
    [SerializeField] private Courses course;
    [SerializeField] private Image questionImage;
    [SerializeField] private GameObject questionImageButton;
    [SerializeField] private string questStringProgress;
    [SerializeField] private LessonsLevelManager levelManager;
    [SerializeField] private GameObject questGiver;
    [SerializeField] private GameObject robotObject;
    private bool started = false;
    GameObject startPanel;
    quizTimer timer;
    //bool isComplete = false;

    #endregion

    #region Public Variables
    public Animator robotAnimator;
    public bool selectedAnswer = false;
    public List<QuestAndAns> QnA;
    public GameObject[] options;
    public TextMeshProUGUI text;
    public GameObject quizPanel;
    public GameObject gameOverPanel;
    public GameObject popUpImage;
    public GameObject winPanel;
    public TextMeshProUGUI[] scoreTxt;
    public TextMeshProUGUI[] rewardTxt;
    [HideInInspector] public int scoreCount;
    public static QuizManager Instance;
    public GameObject[] rewardsPanel;
    public int questionCount = 5;
    [HideInInspector] public int _questionIndex;
    #endregion

    #region Methods/Functions
    public enum Courses {
        [DefaultValue("python")]
        Python,
        [DefaultValue("c++")]
        C,
        [DefaultValue("java")]
        Java,
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable() {
        if (Instance != null)
        {
            Instance = this;
        }

        started = false;
        robotObject.SetActive(true);
    }

    private void OnDisable() {
        robotObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        ShuffleQuestions();
        _totalQuestions = questionCount;
        winPanel.SetActive(false);
        quizPanel.SetActive(true);
        timer = GetComponent<quizTimer>();
        _energy = Energy.Instance.GetCurrentEnergy();

        // Get the default value for the selected enum option
        DefaultValueAttribute[] attributes = (DefaultValueAttribute[])course.GetType().GetField(course.ToString()).GetCustomAttributes(typeof(DefaultValueAttribute), false);
        string defaultValue = (string)attributes[0].Value;

        // Assign the default value to the _currentCourse variable
        _currentCourse = defaultValue;
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
            timer.startGame();
        }
        else
        {
            NPCDialogue.Instance.AddDialogue("I'm exhausted and need to rest. Where can I go to recharge and regain some energy?", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
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
            if (popUpImage != null) popUpImage.SetActive(false);
            timer.resetTime();
            SetCurrentQuestion(_questionIndex);
            Energy.Instance.UseEnergy(1);
            started = true;
        }
        else
        {
            NPCDialogue.Instance.AddDialogue("I'm exhausted and need to rest. Where can I go to recharge and regain some energy?", DataManager.GetPlayerName());
            NPCDialogue.Instance.ShowDialogue();
            return;
        }
    }

    public bool IsStarted() {
        return started;
    }

    public void ShuffleQuestions()
    {
        int n = QnA.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            QuestAndAns temp = QnA[k];
            QnA[k] = QnA[n];
            QnA[n] = temp;
        }
        
        // Add questions
        SetCurrentQuestion(_questionIndex);
    }
    public void GameOver()
    {
        timer.stopTime();
        gameOverPanel.SetActive(true);
        //Lose score index : 0
        SetScoreCount();
    }
    public void Win()
    {
        timer.stopTime();
        winPanel.SetActive(true);
        //Lose score index : 1
        SetScoreCount();
        StartCoroutine(DisplayRewards());
    }

    private void SetScoreCount() {
        foreach (var score in scoreTxt)
        {
            score.text = scoreCount + "/" + _totalQuestions;
        }
    }

    public void OpenPanel()
    {
        if (scoreCount >= _totalQuestions)
        {
            Win();
            RewardManager.Instance.AssessReward();
            StartCoroutine(DelayAddProgress());
            if (levelManager != null) {
                levelManager.addReachedLesson();
            }
        }
        else
        {
            GameOver();
        }
    }

    private IEnumerator DelayAddProgress()
    {
        yield return new WaitForSeconds(3.5f);
        QuestManager.Instance.AddQuestItem(questStringProgress, 1);
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
    private void SetCurrentQuestion(int questionIndex)
    {
        // if (questionIndex >= QnA.Count)
        if (questionIndex >= questionCount)
        {
            OpenPanel();
            return;
        };

        currentQuestion = QnA[questionIndex];

        if(currentQuestion.withImage)
        {
            if (questionImageButton != null) questionImageButton.SetActive(true);
        }
        else 
        {
            if (questionImageButton != null) questionImageButton.SetActive(false);
        }

        text.text = currentQuestion.Questions;

        if (questionImage != null) {
           questionImage.sprite = currentQuestion.QustionImage;
           questionImage.preserveAspect = true;
        }
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

        yield return new WaitForSeconds(.1f);
        if (questGiver != null) {
            questGiver.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            questGiver.SetActive(false);
        }
    }

    #endregion

}
