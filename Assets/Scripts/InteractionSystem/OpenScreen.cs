using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenScreen : MonoBehaviour
{
    public void openScreen(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }

    // test next and back buttons
    public GameObject[] lessons;
    private int currentLesson = 0;

    public void OnNextButtonClicked()
    {
        if (currentLesson < lessons.Length - 1)
        {
            lessons[currentLesson].SetActive(false);
            currentLesson++;
            lessons[currentLesson].SetActive(true);
        }
    }

    public void OnBackButtonClicked()
    {
        if (currentLesson > 0)
        {
            lessons[currentLesson].SetActive(false);
            currentLesson--;
            lessons[currentLesson].SetActive(true);
        }
    }

}
