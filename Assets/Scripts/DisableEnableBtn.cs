using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnableBtn : MonoBehaviour
{
    public GameObject lesson_1;
    public GameObject lesson_2;
    public GameObject lesson_3;
    public GameObject lesson_4;
    public void _1_lesson()
    {
        if (lesson_1.activeInHierarchy == true)
        {
            lesson_1.SetActive(false);
        }
        else
        {
            lesson_1.SetActive(true);
            lesson_2.SetActive(false);
            lesson_3.SetActive(false);
            lesson_4.SetActive(false);
        }
    }
    public void _2_lesson()
    {
        if (lesson_2.activeInHierarchy == true)
        {
            lesson_2.SetActive(false);
        }
        else
        {
            lesson_2.SetActive(true);
            lesson_1.SetActive(false);
            lesson_3.SetActive(false);
            lesson_4.SetActive(false);
        }
    }
    public void _3_lesson()
    {
        if (lesson_3.activeInHierarchy == true)
        {
            lesson_3.SetActive(false);
        }
        else
        {
            lesson_3.SetActive(true);
            lesson_1.SetActive(false);
            lesson_2.SetActive(false);
            lesson_4.SetActive(false);
        }
    }
    public void _4_lesson()
    {
        if (lesson_4.activeInHierarchy == true)
        {
            lesson_4.SetActive(false);
        }
        else
        {
            lesson_4.SetActive(true);
            lesson_1.SetActive(false);
            lesson_2.SetActive(false);
            lesson_3.SetActive(false);
        }
    }
}

