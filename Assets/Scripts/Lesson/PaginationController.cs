using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginationController : MonoBehaviour
{
    public GameObject[] pages;
    public GameObject prevButton;
    public GameObject nextButton;
    private int currentPageIndex = 0;
    public bool isComplete;

    void Start()
    {
        // Hide all pages except the first one
        for (int i = 1; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        // Disable the next button if task is not complete
        if (!isComplete)
        {
            nextButton.SetActive(false);

        }

        // Disable the previous button on the first page
        prevButton.SetActive(false);

    }

    public void NextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            // Hide the current page
            pages[currentPageIndex].SetActive(false);

            // Show the next page
            currentPageIndex++;
            pages[currentPageIndex].SetActive(true);

            // Enable the previous button
            prevButton.SetActive(true);

            MakeNotVisible();

            // Disable the next button on the last page
            if (currentPageIndex == pages.Length - 1)
            {
                nextButton.SetActive(false);
            }

        }
    }

    public void PrevPage()
    {
        if (currentPageIndex > 0)
        {
            // Hide the current page
            pages[currentPageIndex].SetActive(false);

            // Show the previous page
            currentPageIndex--;
            pages[currentPageIndex].SetActive(true);

            // Enable the next button
            nextButton.SetActive(true);
        }

        // Disable the previous button on the first page
        if (currentPageIndex == 0)
        {
            prevButton.SetActive(false);
        }
    }

    // Call this function when the task is completed
    public void MakeVisible()
    {
        isComplete = true;
        nextButton.SetActive(true);

    }

    // Call this function when the task is completed
    public void MakeNotVisible()
    {
        isComplete = false;
        nextButton.SetActive(false);
    }
}
