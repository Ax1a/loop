using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    quizTimer _timer;
    TMP_InputField _input;
    int _currPts = 0;
    public int ptsToWin = 0;
    public GameObject myBlocks;
    public Toggle checkBox;
    public GameObject gameOverPanel;
    public initialPosition blocksInitPos;
    public GameObject startPanel;
    public GameObject WinLosePanel;
    public Transform simpleBlock;
    public TextMeshProUGUI output;
    public static Win Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        gameOverPanel.SetActive(false);
        _timer = GameObject.Find("StartPanel").GetComponent<quizTimer>();
        blocksInitPos = GameObject.Find("resetPos").GetComponent<initialPosition>();
    }
    public void retry()
    {
        startPanel.transform.SetAsLastSibling();
        _timer.resetTime();
        startPanel.transform.localScale = new Vector3(1, 1, 1);
        gameOverPanel.SetActive(false);
        blocksInitPos.ResetPositions();
        _currPts = 0;
    }
    public void validate()
    {
        if (_currPts >= ptsToWin)
        {
            WinLosePanel.transform.gameObject.SetActive(true);
            WinLosePanel.transform.SetAsLastSibling();
            checkBox.GetComponent<Toggle>().isOn = true;
            _timer.stopTime();
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
            Debug.Log("Points: " + _currPts);
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

    public void MinusPoints()
    {
        _currPts -= 1;
        Debug.Log(_currPts);
    }

}
