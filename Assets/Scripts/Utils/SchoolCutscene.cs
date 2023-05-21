using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolCutscene : MonoBehaviour
{
    public bool isAnimating = true;
    public static SchoolCutscene Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public void DoneAnimating() {
        isAnimating = false;
    }

    public void IsAnimating() {
        isAnimating = true;
    }

}
