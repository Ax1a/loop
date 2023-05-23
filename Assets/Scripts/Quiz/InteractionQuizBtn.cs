using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionQuizBtn : MonoBehaviour
{
    public int quizID;
    public string quizScene;
    [SerializeField] private TextMeshProUGUI quizTitleTxt;
    [SerializeField] private TextMeshProUGUI quizDescriptionTxt;
    [SerializeField] private GameObject isCompleteIndicator;

    public void ShowConfirmation() {
        if (InteractionQuizManager.Instance != null) {
            InteractionQuizManager.Instance.ShowInteractionConfirmation(quizScene);
        }
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
