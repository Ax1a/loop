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
            Transform _parent = parent.gameObject.transform.GetChild(0);
            // Change the unchecked box to checked box
            _parent.transform.GetChild(0).gameObject.SetActive(false);
            _parent.transform.GetChild(1).gameObject.SetActive(true);
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
            TutorialManager.Instance.CompletedTutorial();
        }
    }

    
}
