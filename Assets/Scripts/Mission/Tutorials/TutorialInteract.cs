using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteract : Tutorial
{
    private bool isCurrentTutorial = false;
    public List<string> Keys = new List<string>();
    [SerializeField] private GameObject Interactor;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject questGiver;
    InteractObject interactObject;

    private void Start() {
        interactObject = Interactor.GetComponent<InteractObject>();
    }

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;

        if (Input.inputString.Contains(Keys[0]) && interactObject.NearInteractable()) {
            Keys.RemoveAt(0);
            // Change the toggle box to check
            Transform _panel = parent.gameObject.transform.GetChild(0);
            QuestUISidePanel _questUI = _panel.GetComponent<QuestUISidePanel>();
            _questUI.ToggleCheck(true, 0);
            //
            AudioManager.Instance.PlaySfx("Success");
        }

        if (Keys.Count == 0){
            // QuestManager.Instance.QuestRequest(questGiver.GetComponent<QuestObject>());
            // Debug.Log(QuestManager.Instance.CheckAcceptedQuest(questGiver.GetComponent<QuestObject>()));
            int objCount = parent.transform.childCount;

            if(objCount == 0) return;

            foreach (Transform child in parent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            BotGuide.Instance.AddDialogue("Great job! You've got the hang of it. Remember, you can interact with lots of different objects throughout the game,"); 
            BotGuide.Instance.AddDialogue("so keep your eyes peeled for that 'Interact' prompt. Happy exploring!"); 
            BotGuide.Instance.ShowDialogue();
            ShowCompletedTutorial();
            TutorialManager.Instance.CompletedTutorial();
            StartCoroutine(DelayAddQuest());
        }
    }

    private void ShowCompletedTutorial() {
        // Create new instance of the object class
        Quest _quest = new Quest();

        // Add values to the object
        _quest.title = "Basic Tutorial";
        _quest.questType = Quest.QuestType.MAIN;

        // Call the banner popup class
        QuestUI.Instance.ShowCompleteQuestBanner(_quest);
    }

    // For testing only // Fix add queue on popup
    private IEnumerator DelayAddQuest() {
        yield return new WaitForSeconds(3.5f);
        questGiver.SetActive(true);
    }    
}
