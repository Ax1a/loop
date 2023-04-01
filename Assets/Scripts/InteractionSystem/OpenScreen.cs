using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] taskbarIcons;

    void Update ()
    {
        OnClickedAudio();
    }
    public void openScreen(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            if (panel.name == "Lesson Screen") taskbarIcons[0].SetActive(!isActive);
            panel.SetActive(!isActive);
        }
    }
    public void MinimizeScreen(GameObject panel) {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
        }
    }

    public void OnClickedAudio()
    {
        GameObject _computerCanvas = GameObject.FindGameObjectWithTag("mainCanvas");
        if (_computerCanvas.activeSelf)
        {
            if(Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlaySfx("Click");
            }
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
