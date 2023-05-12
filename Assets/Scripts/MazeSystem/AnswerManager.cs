using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    public bool isCorrect = false;
    public MazeQuizManager mazeQuizManager;
    
    public void Answer ()
    {
        if(isCorrect)
        {
            mazeQuizManager.Correct();
        }
        else 
        {
            mazeQuizManager.Wrong();

        }
    }
}
