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
        addReachedLesson();
        GameSharedUI.Instance.updateButtonDisabled();
        reachedLesson = DataManager.getReachedLesson();
        Debug.Log(reachedLesson);
       
    }
    void Update()
    {
        
    }

    // public void addLesson()
    // {
    //     addReachedLesson();
   

    //     //updateButtonDisabled();
    // }
    public void resetLesonLevel()
    {
        DataManager.resetLevel();
        GameSharedUI.Instance.updateButtonDisabled();
        Debug.Log("Level Reset");

    }

    public void addReachedLesson()
    {
      
        DataManager.addReachedLesson(1);
        GameSharedUI.Instance.updateButtonDisabled();
        Debug.Log(reachedLesson);

      
        // updateButtonDisabled();
    }

    // public void updateButtonDisabled()
    // {
    //     foreach (Button btn in lessonBtn)
    //     {
    //         btn.interactable = false;
    //     }

    //     if (lessonBtn.Length >= reachedLesson)
    //     {
    //         for (int i = 0; i < reachedLesson; i++)
    //         {
    //             lessonBtn[i].interactable = true;
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("No Levels Left");
    //     }
    // }
}
