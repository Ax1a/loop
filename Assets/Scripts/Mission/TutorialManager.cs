using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public List<Tutorial> Tutorials = new List<Tutorial>();


    [Header("Tutorial Objects")]
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI requirementText;
    public GameObject tutorialPrefab;

    private static TutorialManager _instance;
    public static TutorialManager Instance {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<TutorialManager>();

            if (_instance == null)
                Debug.Log("There is no tutorial manager");

            return _instance;
        }
    }

    private Tutorial currentTutorial;

    private void Start() {
        SetNextTutorial(DataManager.GetQuestProgress());
    }

    private void Update() {
        if (currentTutorial) currentTutorial.CheckIfHappening();
    }

    public void CompletedTutorial() {
        SetNextTutorial(currentTutorial.Order + 1);
        DataManager.SetQuestProgress(1);
        Debug.Log("Completed");
    }

    public void SetNextTutorial(int currentOrder) {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if(!currentTutorial) {
            CompletedAllTutorials();
            return;
        }

        descriptionText.text = currentTutorial.Explanation;        
        titleText.text = currentTutorial.Title;
        Transform parent = GameObject.Find("Contents").transform;

        foreach (var req in currentTutorial.Requirement)
        {
            TextMeshProUGUI requirement = Instantiate(requirementText, parent.transform);    
            requirement.text = req;        
            // requirement.transform.parent = parent.transform;
        }

        if(parent.childCount != 0) GameObject.Destroy(parent.GetChild(0).gameObject);
    } 

    public void CompletedAllTutorials() {
        descriptionText.text = "You have completed all the tutorials";
        tutorialPrefab.SetActive(false);
    }

    public Tutorial GetTutorialByOrder(int Order) {
        for (int i = 0; i < Tutorials.Count; i++) {
            if (Tutorials[i].Order == Order) return Tutorials[i];
        }

        return null;
    }
}
