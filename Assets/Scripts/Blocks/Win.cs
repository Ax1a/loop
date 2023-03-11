using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public int ptsToWin = 0;
    int _currPts = 0;
    public GameObject myBlocks;
    public Toggle checkBox;
    public GameObject gameOverPanel;
    quizTimer _timer;
    public initialPosition blocksInitPos;
    public GameObject startPanel;
    public GameObject WinLosePanel;
    public Transform simpleBlock;
    TMP_InputField _input;
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
        _timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        blocksInitPos = GameObject.Find("resetPos").GetComponent<initialPosition>();
        //drag = GameObject.Find("DragScript").GetComponent<Drag>();
    }
    public void retry()
    {
        startPanel.transform.SetAsLastSibling();
        _timer.resetTime();
        startPanel.transform.localScale = new Vector3(1, 1, 1);
        transform.GetChild(1).gameObject.SetActive(false);
        gameOverPanel.SetActive(false);
        blocksInitPos.ResetPositions();
        Debug.Log("Retry");
    }
    public void validate()
    {
        if (_currPts >= ptsToWin)
        {
            WinLosePanel.transform.gameObject.SetActive(true);
            WinLosePanel.transform.SetAsLastSibling();
            checkBox.GetComponent<Toggle>().isOn = true;
            _timer.stopTime();
            Debug.Log("Winner");
            Debug.Log("Points: " + _currPts);

            RewardManager.Instance.AssessReward();

            if (simpleBlock != null)
            {
                _input = simpleBlock.GetComponentInChildren<TMP_InputField>();
                output.text = _input.text;
            }
        }
        else
        {
            Debug.Log("Losser");
            gameOverPanel.transform.SetAsLastSibling();
            gameOverPanel.SetActive(true);
            _timer.stopTime();
        }
    }
    public void AddPoints()
    {
        _currPts += 1;
        Debug.Log(_currPts);
    }

}
