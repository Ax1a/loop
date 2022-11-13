using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] private GameObject LessonScreen; 
    [SerializeField] private GameObject DesktopScreen; 
    public void openScreen() {
        if (LessonScreen != null)
        {
            bool isActive = LessonScreen.activeSelf;

            DesktopScreen.SetActive(isActive);
            LessonScreen.SetActive(!isActive);
        }
    }
}
