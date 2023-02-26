using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonsLevelManager : MonoBehaviour
{
    public Button[] lessonBtn;
    int reachedLesson;


    void Start()
    {
        reachedLesson = DataManager.getReachedLesson();
        updateButtonDisabled();
        // addReachedLesson();
        Debug.Log("reached lesson" + reachedLesson);

    }

    //for testing only 
    //must remove in final build
    public void resetLesonLevel()
    {
        DataManager.resetLevel();
        reachedLesson = 1;
        updateButtonDisabled();
        Debug.Log("Level Reset");
        Debug.Log("reached lesson" + reachedLesson);

    }
//function for adding level
    public void addReachedLesson()
    {
        DataManager.addReachedLesson();
        reachedLesson++;
        updateButtonDisabled();
    }

//disable all button
//activate if level is reached.
    public void updateButtonDisabled()
    {
        foreach (Button btn in lessonBtn)
        {
            btn.interactable = false;
        }

        if (lessonBtn.Length >= reachedLesson)
        {
            for (int i = 0; i < reachedLesson; i++)
            {
                lessonBtn[i].interactable = true;
            }
        }
        else
        {
            foreach (Button btn in lessonBtn)
            {
                btn.interactable = true;
            }
            Debug.Log("No Levels Left");
        }
    }
}
