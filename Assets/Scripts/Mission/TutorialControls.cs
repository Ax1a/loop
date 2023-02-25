using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControls : Tutorial
{
    public List<string> Keys = new List<string>();
    [SerializeField] private GameObject contents;
    
    public override void CheckIfHappening()
    {
        for (int i = 0; i < Keys.Count; i++) {
            if (Input.inputString.Contains(Keys[i])) {
                Keys.RemoveAt(i);

                // Add change color text here when completed
                if (Input.inputString == "b") {
                    DataManager.AddMoney(20);
                    BotGuide.Instance.AddDialogue("Great! You have been given 20 currency units to spend."); 
                    BotGuide.Instance.AddDialogue("Perhaps you could treat yourself to a nice cup of coffee? Try clicking the buy button of an item.");
                    BotGuide.Instance.ShowDialogue();
                }
                if (Input.inputString == "i") {
                    BotGuide.Instance.AddDialogue("Great job! In the inventory, you can browse your items, or use them if needed.");
                    BotGuide.Instance.AddDialogue("Now let's close the inventory to continue the tutorial.");
                    BotGuide.Instance.ShowDialogue();
                }
                break;
            }
        }

        if (Keys.Count == 0){
            if (contents.activeSelf == true) {
                Transform parent = contents.transform;
                int objCount = parent.childCount;

                if(objCount == 0) return;

                foreach (Transform child in parent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                TutorialManager.Instance.CompletedTutorial();
            }
        }
    }
}
