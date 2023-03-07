using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public int ptsToWin = 0;
    private int currPts = 0;
    public GameObject myBlocks;
    public Toggle checkBox;
    public GameObject gameOverPanel;
    quizTimer timer;
    public initialPosition blocksInitPos;
    public GameObject startPanel;
    public GameObject WinLosePanel;
    public Transform simpleBlock;
    private TMP_InputField input;
    public TextMeshProUGUI output;

    public static Win Instance;
    //[HideInInspector] public Drag drag;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        // ptsToWin = myBlocks.transform.childCount;
        gameOverPanel.SetActive(false);
        timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        blocksInitPos = GameObject.Find("resetPos").GetComponent<initialPosition>();
        //drag = GameObject.Find("DragScript").GetComponent<Drag>();
    }
    public void retry()
    {
        startPanel.transform.SetAsLastSibling();
        timer.resetTime();
        startPanel.transform.localScale = new Vector3(1, 1, 1);
        transform.GetChild(1).gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        blocksInitPos.ResetPositions();
        Debug.Log("Retry");
    }
    public void validate()
    {
        if (currPts >= ptsToWin)
        {
            WinLosePanel.transform.gameObject.SetActive(true);
            WinLosePanel.transform.SetAsLastSibling();
            checkBox.GetComponent<Toggle>().isOn = true;
            timer.stopTime();
            Debug.Log("Winner");
            Debug.Log("Points: " + currPts);

            RewardManager.Instance.AssessReward();

            if (simpleBlock != null)
            {
                input = simpleBlock.GetComponentInChildren<TMP_InputField>();
                output.text = input.text;
            }
        }
        else
        {
            Debug.Log("Losser");
            gameOverPanel.transform.SetAsLastSibling();
            gameOverPanel.SetActive(true);
            timer.stopTime();
        }
    }
    public void AddPoints()
    {
        currPts += 1;
        Debug.Log(currPts);
    }

    public float ComputedReward(int score, float time)
    {
        // Calculate the raw score based on the player's score and time
        float rawScore = score * Mathf.Pow(2f, -time);

        // Compute the reward by rounding the raw score to the nearest integer
        // int reward = Mathf.RoundToInt(rawScore);

        return rawScore;
    }

}
