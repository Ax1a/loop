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
    [SerializeField] private GameObject questPanelPrefab;
    [SerializeField] private Transform parentContainer;
    private bool _activated = false;

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
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart() {
        yield return new WaitForSeconds(.5f);

        if(DataManager.GetQuestProgress() == 0) {
            BotGuide.Instance.AddDialogue("Welcome to the game! Before you get started, let's go over the controls."); 
            BotGuide.Instance.AddDialogue("You can move your character with the arrow keys or WASD."); 
            BotGuide.Instance.ShowDialogue();
        }
    }

    private void Update() {
        if (currentTutorial && BotGuide.Instance.guideIsActive() == false) currentTutorial.CheckIfHappening();
        if (UIController.Instance.otherPanelActive() == true) return;
        
        if (DataManager.GetQuestProgress() == 2 && _activated == false) {
            BotGuide.Instance.AddDialogue("You can interact with certain objects in the game using the E key."); 
            BotGuide.Instance.AddDialogue("When you see an object with the 'Interact' prompt above it, just press E to interact with it. Give it a try on that nearby object now!"); 
            BotGuide.Instance.ShowDialogue();
            _activated = true;
        }
    }

    public void CompletedTutorial() {
        DataManager.SetQuestProgress(1);
        SetNextTutorial(currentTutorial.Order + 1);

        if (DataManager.GetQuestProgress() == 1) {
            BotGuide.Instance.AddDialogue("To access the Shop, press the 'B' key on your keyboard. To open your Inventory, press the 'I' key."); 
            BotGuide.Instance.ShowDialogue();
        }
    }

    public void SetNextTutorial(int currentOrder) {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if(!currentTutorial) {
            CompletedAllTutorials();
            return;
        }

        // descriptionText.text = currentTutorial.Explanation;        
        // titleText.text = currentTutorial.Title;
        // Setup the side panel UI
        if(parentContainer.childCount != 0) GameObject.Destroy(parentContainer.GetChild(0).gameObject);

        GameObject sidePanelQuest = Instantiate(questPanelPrefab, parentContainer); // Create a new gameobject based on prefab
        QuestUISidePanel qUISidePanel = sidePanelQuest.GetComponent<QuestUISidePanel>(); // Call the script from prefab
        qUISidePanel.SetQuestTitle(currentTutorial.Title); // Set the quest title
        qUISidePanel.SetQuestSubTitle(currentTutorial.Explanation); // Set the quest description
        qUISidePanel.GenerateObjectives(currentTutorial.Requirement); // Generate the quest objectives
        // foreach (var req in currentTutorial.Requirement)
        // {
        //     GameObject requirement = Instantiate(contentPrefab, parentContainer.transform);    
        //     requirement.GetComponentInChildren<TextMeshProUGUI>().text = req;    
            // TextMeshProUGUI requirement = Instantiate(requirementText, parent.transform);    
            // requirement.text = req;        
            // requirement.transform.parent = parent.transform;
        // }

    } 

    public void CompletedAllTutorials() {
        descriptionText.text = "You have completed all the tutorials";
        titleText.text = "Tutorial Complete";
        if(parentContainer.transform.childCount != 0) GameObject.Destroy(parentContainer.transform.GetChild(0).gameObject);
        // tutorialPrefab.SetActive(false);
        SaveGame.Instance.SaveGameState();
    }

    public Tutorial GetTutorialByOrder(int Order) {
        for (int i = 0; i < Tutorials.Count; i++) {
            if (Tutorials[i].Order == Order) return Tutorials[i];
        }

        return null;
    }
}
