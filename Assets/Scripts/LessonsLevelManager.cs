using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonsLevelManager : MonoBehaviour
{
    GameSharedUI gameSharedUI;
    void Start()
    {
        DataManager.addReachedLesson(1);


    }
    void Update()
    {
        GameSharedUI.Instance.updateButtonDisabled();
    }
    public void resetLesonLevel()
    {
        DataManager.resetLevel();
        Debug.Log("Level Reset");

    }

    public void levelComplete()
    {

        DataManager.addReachedLesson(1);

    }
}
