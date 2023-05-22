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
    [SerializeField] private GameObject confirmation;

    public void ShowConfirmation() {
        confirmation.SetActive(true);
        confirmation.transform.SetParent(gameObject.transform.parent.transform.parent.transform.parent);
        confirmation.transform.position = new Vector3(550f, 290f, 0);
        confirmation.transform.SetAsLastSibling();
    }

    public void LoadMaze() {
        SaveGame.Instance.SaveGameState();
        InteractionQuizManager.Instance.LoadSceneMaze(quizScene);
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
