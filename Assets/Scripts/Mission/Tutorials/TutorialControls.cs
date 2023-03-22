using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControls : Tutorial
{
    public List<string> Keys = new List<string>();
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject highlightGuide;
    private List<string> _keyCountTemp;

    private void OnEnable() {
        _keyCountTemp = Keys.ToList();
    }

    public override void CheckIfHappening()
    {
        for (int i = 0; i < _keyCountTemp.Count; i++) {
            if (Input.inputString.Contains(_keyCountTemp[i])) {
                // Change the toggle box to check
                Transform _panel = parent.gameObject.transform.GetChild(0);
                QuestUISidePanel _questUI = _panel.GetComponent<QuestUISidePanel>();
                _questUI.ToggleCheck(true, i);
            }
        }

        for (int i = 0; i < Keys.Count; i++) {
            if (Input.inputString.Contains(Keys[i])) {
                AudioManager.Instance.PlaySfx("Success");
                Keys.RemoveAt(i);

                if (Input.inputString == "b") {
                    DataManager.AddMoney(15);
                    BotGuide.Instance.AddDialogue("Great! You have been given 15 currency units to spend."); 
                    BotGuide.Instance.AddDialogue("Perhaps you could treat yourself to a nice cup of coffee? Try clicking the buy button of an item.");
                    BotGuide.Instance.ShowDialogue();
                    UIController.Instance.EnqueuePopup(highlightGuide);
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
