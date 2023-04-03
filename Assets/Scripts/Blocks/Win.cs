using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [Header ("Quiz Interaction Panels")]
    public GameObject gameOverPanel;
    public GameObject startPanel;
    public GameObject WinLosePanel;

    [Header ("Quiz Interaction Objects")]
    public Toggle checkBox;
    public initialPosition blocksInitPos;
    public Transform simpleBlock;
    public TextMeshProUGUI output;
    [SerializeField] private GameObject instructionTxt;
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform blockPlaceholdersParent;

    [Header ("Reward Objects")]
    [SerializeField] private quizTimer quizTimerScript;
    [SerializeField] private TextMeshProUGUI moneyTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private GameObject[] popupGameObjects;
    [SerializeField] private InteractionQuizInfo interactionQuiz;

    private int _ptsToWin = 0;
    private int _currPts = 0;
    TMP_InputField _input;

    void OnEnable()
    {
        gameOverPanel.SetActive(false);
        // blocksInitPos = GameObject.Find("resetPos").GetComponent<initialPosition>();

        _ptsToWin = blockPlaceholdersParent.childCount;
    }
    public void retry()
    {
        startPanel.transform.SetAsLastSibling();
        quizTimerScript.stopTime();
        quizTimerScript.resetTime();
        startPanel.transform.localScale = new Vector3(1, 1, 1);
        gameOverPanel.SetActive(false);

        // Add reset position on retry
        _currPts = 0;
    }
    public void validate()
    {
        if (_currPts >= _ptsToWin)
        {
            Debug.Log("Points: " + _currPts);
            checkBox.GetComponent<Toggle>().isOn = true;
            
            WinLosePanel.transform.gameObject.SetActive(true);
            WinLosePanel.transform.SetAsLastSibling();

            StartCoroutine(DisplayObjects());

            quizTimerScript.stopTime();
            RewardManager.Instance.AssessReward();
            InteractionQuizManager.Instance.SetInteractionAsComplete(interactionQuiz);

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
            quizTimerScript.stopTime();
        }
    }
    public void AddPoints()
    {
        if (instructionTxt != null) instructionTxt.SetActive(false);
        _currPts += 1;
        Debug.Log(_currPts);
    }

    public void MinusPoints()
    {
        if (instructionTxt != null) instructionTxt.SetActive(false);
        _currPts -= 1;
        Debug.Log(_currPts);
    }

    private IEnumerator DisplayObjects() {
        // Add set of money and exp values here

        foreach (var obj in popupGameObjects)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
    } 

}
