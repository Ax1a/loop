using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LessonDragDropValidation : Singleton<LessonDragDropValidation>
{
    private int _currPts = 0;
    public int CurrPts
    {
        get { return _currPts; }
        set { _currPts = value; }
    }
    public int ptsToWin = 0;
    string _playerName;
    [HideInInspector] public bool isCorrect = false;
    public LessonInitPos initPos;
    GameObject rootPanel;


    void Start()
    {
        _playerName = DataManager.GetPlayerName();
        rootPanel = GameObject.FindGameObjectWithTag("root");

    }
    public void Validate()
    {
        if (CurrPts >= ptsToWin)
        {
            NPCCall("Great! Seems like I understand the lesson");
            isCorrect = true;
            Debug.Log("You got it right");
        }
        else
        {
            NPCCall("I better review the lesson again! :<");
            isCorrect = false;
            Debug.Log("There is something wrong");
        }
    }
    public void IsCorrect()
    {
        if (CurrPts >= ptsToWin)
        {
            isCorrect = true;
            Debug.Log("isCorrect is true");
        }
    }

    public void Reset()
    {
        CurrPts = 0;
        initPos.ResetPositions();
        LessonDropBlock.Instance._pointsAdded = false;
        isCorrect = false;
        // reset each LessonDropBlock object in the scene
        foreach (LessonDropBlock dropBlock in FindObjectsOfType<LessonDropBlock>())
        {
            dropBlock.Reset();
        }
        LayoutRefresher.Instance.RefreshContentFitter((RectTransform)rootPanel.transform);

    }

    public void AddPoints()
    {
        CurrPts += 1;
        Debug.Log("Current points: " + CurrPts);

    }
    public void MinusPoints()
    {
        CurrPts -= 1;
        if (CurrPts < 0)
        {
            CurrPts = 0;
        }
        Debug.Log("Current points: " + CurrPts);

    }

    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
