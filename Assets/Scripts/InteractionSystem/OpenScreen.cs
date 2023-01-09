using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] private GameObject LessonScreen; 
    [SerializeField] private GameObject DesktopScreen;
    [SerializeField] private GameObject QuizScreen;
    [SerializeField] private GameObject QuizScreen_2;
    [SerializeField] private GameObject PGScreen;
    // [SerializeField] public  GameObject quizContent;

    public void openLesson() {
        if (LessonScreen != null)
        {
            bool isActive = LessonScreen.activeSelf;

            // DesktopScreen.SetActive(isActive);
            LessonScreen.SetActive(!isActive);
        }
    }

    public void openQuiz()
    {
        if (QuizScreen != null)
        {
            bool isActive = QuizScreen.activeSelf;

            // DesktopScreen.SetActive(isActive);
            QuizScreen.SetActive(!isActive);
        }
        
        //quizContent.SetActive(true);
    
    }

    public void openQuiz_2()
    {
        if (QuizScreen_2 != null)
        {
            bool isActive = QuizScreen.activeSelf;

            // DesktopScreen.SetActive(isActive);
            QuizScreen_2.SetActive(!isActive);
        }
        
        //quizContent.SetActive(true);
    
    }

    public void openPG()
    {
        if (PGScreen != null)
        {
            bool isActive = PGScreen.activeSelf;

            // DesktopScreen.SetActive(isActive);
            PGScreen.SetActive(!isActive);
        }
    }






}
