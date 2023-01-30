using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public int Order;

    public string Title;
    public string Explanation;
    public string[] Requirement;


    private void Awake() {
        TutorialManager.Instance.Tutorials.Add(this);
    }

    public virtual void CheckIfHappening() {}
}
