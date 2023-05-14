using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionQuizBtn : MonoBehaviour
{
    public int quizID;
    [SerializeField] private TextMeshProUGUI quizTitleTxt;
    [SerializeField] private TextMeshProUGUI quizDescriptionTxt;
    [SerializeField] private GameObject isCompleteIndicator;

    public void ShowQuizPanel() {
        // change to string
        InteractionQuizManager.Instance.LoadSceneMaze(quizID);
    }

    public void ShowCompleteIndicator() {
        isCompleteIndicator.SetActive(true);
    }

    public void SetQuizTitle(string title) {
        // quizTitle = title;
        quizTitleTxt.text = title;
    } 

    public void SetQuizDescription(string description) {
        // quizDescription = description;
        quizDescriptionTxt.text = description;
    }
} 
