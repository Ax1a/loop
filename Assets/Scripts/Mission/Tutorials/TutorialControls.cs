using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControls : Tutorial
{
    public List<string> Keys = new List<string>();
    [SerializeField] private GameObject parent;
    
    private void Start() {
    }

    public override void CheckIfHappening()
    {
        for (int i = 0; i < Keys.Count; i++) {
            if (Input.inputString.Contains(Keys[i])) {
                Transform _parent = parent.gameObject.transform.GetChild(i);
                Keys.RemoveAt(i);
                // Change the unchecked box to checked box
                _parent.transform.GetChild(0).gameObject.SetActive(false);
                _parent.transform.GetChild(1).gameObject.SetActive(true);
                AudioManager.Instance.PlaySfx("Success");

                // Add change color text here when completed
                if (Input.inputString == "b") {
                    //AudioManager.Instance.PlaySfx("Success");
                    DataManager.AddMoney(15);
                    BotGuide.Instance.AddDialogue("Great! You have been given 15 currency units to spend."); 
                    BotGuide.Instance.AddDialogue("Perhaps you could treat yourself to a nice cup of coffee? Try clicking the buy button of an item.");
                    BotGuide.Instance.ShowDialogue();
                }
                if (Input.inputString == "i") {
                   // AudioManager.Instance.PlaySfx("Success");
                    BotGuide.Instance.AddDialogue("Great job! In the inventory, you can browse your items, or use them if needed.");
                    BotGuide.Instance.AddDialogue("Now let's close the inventory to continue the tutorial.");
                    BotGuide.Instance.ShowDialogue();
                }
                break;
            }
        }

        if (Keys.Count == 0){
            if (parent.activeSelf == true) {
                Transform _parent = parent.transform;
                int objCount = _parent.childCount;

                if(objCount == 0) return;

                foreach (Transform child in _parent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                TutorialManager.Instance.CompletedTutorial();
            }
        }
    }
}
