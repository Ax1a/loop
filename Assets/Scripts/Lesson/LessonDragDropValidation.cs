using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonDragDropValidation : Singleton<LessonDragDropValidation>
{
    [HideInInspector] public int _currPts = 0;
    public int ptsToWin = 0;
    string _playerName;
    [HideInInspector] public bool isCorrect = false;
    public LessonInitPos initPos;

    void Start ()
    {
        _playerName = DataManager.GetPlayerName();
    }
    public void Validate()
    {
        if (_currPts >= ptsToWin)
        {
            NPCCall("Great! Seems like I understand the lesson");
            Debug.Log("You got it right");
        }
        else
        {
            NPCCall("I better review the lesson again! :<");
            Debug.Log("There is something wrong");
        }
    }

    public void Reset()
    {
        _currPts = 0;
        Debug.Log(_currPts);
        initPos.ResetPositions();
        Debug.Log("Reset");
        LessonDropBlock.Instance._pointsAdded = false;
    }

    public void AddPoints()
    {
        _currPts += 1;
        Debug.Log(_currPts);
    }

    public void MinusPoints()
    {
        _currPts -= 1;

        if (_currPts < 0)
        {
            _currPts = 0;
            Debug.Log(_currPts);
        }
        Debug.Log(_currPts);
        Debug.Log("points minus");
    }

    void NPCCall(string message)
    {
        NPCDialogue.Instance.AddDialogue(message, _playerName);
        NPCDialogue.Instance.ShowDialogue();
    }
}
