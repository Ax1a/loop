using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteract : Tutorial
{
    private bool isCurrentTutorial = false;
    public List<string> Keys = new List<string>();

    public override void CheckIfHappening()
    {
        isCurrentTutorial = true;

        if (Input.inputString.Contains(Keys[0])) Keys.RemoveAt(0);

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
