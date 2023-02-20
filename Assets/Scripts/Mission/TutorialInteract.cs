using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteract : Tutorial
{
    private bool isCurrentTutorial = false;
    [SerializeField] private GameObject Interactor;
    public List<string> Keys = new List<string>();
    InteractObject interactObject;

    private void Start() {
        interactObject = Interactor.GetComponent<InteractObject>();
    }

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;

        if (Input.inputString.Contains(Keys[0]) && interactObject.NearInteractable()) {
            Keys.RemoveAt(0);
        }

        if (Keys.Count == 0){
            Transform parent = GameObject.Find("Contents").transform;
            int objCount = parent.childCount;

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
