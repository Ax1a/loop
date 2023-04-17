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

    void Start()
    {
        _playerName = DataManager.GetPlayerName();
    }
    public void Validate()
    {
        if (CurrPts >= ptsToWin)
        {
            NPCCall("Great! Seems like I understand the lesson");
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
        }
    }

    public void Reset()
    {
        CurrPts = 0;
        initPos.ResetPositions();
        LessonDropBlock.Instance._pointsAdded = false;
        isCorrect = false;
        LayoutRefresher.Instance.RefreshContentFitter(transform.parent as RectTransform);
    }

    public void AddPoints()
    {
        CurrPts += 1;

    }

    public void MinusPoints()
    {
        CurrPts -= 1;

        if (CurrPts < 0)
        {
            CurrPts = 0;
        }

    }

    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
