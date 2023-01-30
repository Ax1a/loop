using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControls : Tutorial
{
    public List<string> Keys = new List<string>();

    public override void CheckIfHappening()
    {
        for (int i = 0; i < Keys.Count; i++) {
            if (Input.inputString.Contains(Keys[i])) {
                Keys.RemoveAt(i);

                // Add change color text here when completed
                break;
            }
        }

        if (Keys.Count == 0){
            Transform parent = GameObject.Find("Contents").transform;
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
