using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] private GameObject LessonScreen; 
    [SerializeField] private GameObject DesktopScreen;
    [SerializeField] private GameObject QuizScreen;
    [SerializeField] private GameObject PGScreen;

    public void openScreen() {
        if (LessonScreen != null)
        {
            bool isActive = LessonScreen.activeSelf;

            DesktopScreen.SetActive(isActive);
            LessonScreen.SetActive(!isActive);
        }
    }

    public void openQuiz()
    {
        if (QuizScreen != null)
        {
            bool isActive = QuizScreen.activeSelf;

            DesktopScreen.SetActive(isActive);
            QuizScreen.SetActive(!isActive);
        }
    }

    public void openPG()
    {
        if (PGScreen != null)
        {
            bool isActive = PGScreen.activeSelf;

            DesktopScreen.SetActive(isActive);
            PGScreen.SetActive(!isActive);
        }
    }






}
