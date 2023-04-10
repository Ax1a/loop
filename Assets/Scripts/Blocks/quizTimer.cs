using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class quizTimer : MonoBehaviour
{
    public enum Difficulty { easy, medium, hard }
    public enum QuizType { Interactive, Multiple  };

    [Header ("Params")]
    public Difficulty difficulty;
    public QuizType quizType;
    public float currTime = 0f;
    public float startingTime = 0f;
    [HideInInspector] public bool isStart = false;

    [Header ("Quiz Game Objects")]
    public GameObject gameOverPanel;
    public GameObject startPanel;
    [SerializeField] public TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI quizTitle;
    [Header ("For Interaction Quiz Only")]
    [SerializeField] private InteractionQuizInfo interactionInfo;
    public static quizTimer Instance;
    int _energy;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        //Define starting time based on difficulty levels.. 
        if (difficulty == Difficulty.easy)
        {
            startingTime = 20f;
        }
        else if (difficulty == Difficulty.medium)
        {
            startingTime = 30f;
        }
        else
        {
            startingTime = 60f;
        }

        currTime = startingTime;
        gameOverPanel.SetActive(false);
    }

    private void OnEnable() {
        if (interactionInfo != null) quizTitle.text = interactionInfo.data.quizTitle;
    }
    
    // Update is called once per frame    
    public void startGame()
    {
        if (!isStart)
        {
            // currTime = startingTime;
            if (Energy.Instance.GetCurrentEnergy() > 0)
            {
                Energy.Instance.UseEnergy(1);
                isStart = true;
                // startPanel.transform.localScale = new Vector3(0, 0, 0);
                startPanel.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("No Energy Left");
                return;
            }
        }

    }
    public void stopTime()
    {
        //Check if timer is starting
        if (isStart)
        {
            Debug.Log("Timer Stop");
            isStart = false;
        }
    }
    public void resetTime()
    {
        currTime = startingTime;
        isStart = true;
        Debug.Log("Reset the timer");
    }

    public void res()
    {
        isStart = false;
    }
    void Update()
    {
        //Decrease the time when the timer is starting. 
        //The float type is converted to string
        //When current time is equal to zero, it activate game over panel.
        if (isStart)
        {
            currTime -= 1 * Time.deltaTime;
            float time = currTime;
            countDownText.text = time.ToString("0");

            if (currTime <= 0)
            {
                currTime = 0;
                // gameOverPanel.SetActive(true);
                if(quizType == QuizType.Multiple)
                {
                    Debug.Log("Multiple Choice Quiz: GameOver");
                    // QuizManager.Instance.GameOver();
                    gameOverPanel.SetActive(true);
                }
                else if (quizType == QuizType.Interactive)
                {
                    Debug.Log("Interactive Quiz: GameOver");
                    Win.Instance.GameOver();
                }
                gameOverPanel.transform.SetAsLastSibling();
                isStart = false;
            }
        }

    }


}
