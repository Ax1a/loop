using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonsLevelManager : MonoBehaviour
{
    [Header("Instances")]
    public Button[] lessonBtn;
    [SerializeField] private GameObject[] headers;
    [SerializeField] private GameObject[] progressIndicators;

    [Header("Params")]
    [SerializeField] private int easyLevelCount;
    [SerializeField] private int mediumLevelCount;
    [SerializeField] private int difficultLevelCount;
    private Progress _easy, _medium, _difficult;
    [SerializeField] CurrentCourse currentCourse;
    int reachedLesson;
    int level;
    public string course;
    public static LessonsLevelManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GetCurrentCourse();
        UpdateLessonState();
        // addReachedLesson();
        Debug.Log("reached lesson" + reachedLesson);

    }

    void Update()
    {
        if (reachedLesson != DataManager.GetProgrammingLanguageProgress(course))
        {
            reachedLesson = DataManager.GetProgrammingLanguageProgress(course);
        }
    }

    //This will get current course taking
    void GetCurrentCourse()
    {
        if (currentCourse == CurrentCourse.c)
        {
            course = "c++";
        }
        else if (currentCourse == CurrentCourse.python)
        {
            course = "python";
        }
        else
        {
            course = "java";
        }
    }

    //for testing only 
    //must remove in final build
    public void resetLesonLevel()
    {
        DataManager.resetLevel();
        reachedLesson = 1;
        UpdateLessonState();
        Debug.Log("Level Reset");
        Debug.Log("reached lesson" + reachedLesson);

    }
    //function for adding level
    public void addReachedLesson()
    {
        DataManager.addReachedLesson();
        reachedLesson++;
        UpdateLessonState();
    }

    //disable all button
    //activate if level is reached.
    public void UpdateLessonState()
    {
        DisplayProgressState();

        foreach (Button btn in lessonBtn)
        {
            Image image = btn.transform.GetChild(3).GetComponent<Image>();
            image.gameObject.SetActive(true);
            btn.interactable = false;
        }

        if (lessonBtn.Length >= reachedLesson)
        {
            for (int i = 0; i < reachedLesson; i++)
            {
                Image image = lessonBtn[i].transform.GetChild(3).GetComponent<Image>();
                image.gameObject.SetActive(false);
                lessonBtn[i].interactable = true;
            }
        }
        else
        {
            foreach (Button btn in lessonBtn)
            {
                btn.interactable = true;
                Image image = btn.transform.GetChild(3).GetComponent<Image>();
                image.gameObject.SetActive(true);
            }
            Debug.Log("No Levels Left");
        }
    }

    // Update Difficulty Level Icon
    private void DisplayProgressState()
    {
        // Conditions to check based on the reached lessons
        if (DataManager.GetProgrammingLanguageProgress(course) <= easyLevelCount)
        {
            UpdateProgressEnums(0, 2, 2);
        }
        else if (DataManager.GetProgrammingLanguageProgress(course) >= easyLevelCount + mediumLevelCount + difficultLevelCount)
        {
            UpdateProgressEnums(1, 1, 1);
        }
        else if (DataManager.GetProgrammingLanguageProgress(course) > mediumLevelCount + easyLevelCount)
        {
            UpdateProgressEnums(1, 1, 0);
        }
        else if (DataManager.GetProgrammingLanguageProgress(course) > easyLevelCount)
        {
            UpdateProgressEnums(1, 0, 2);
        }

        // Display the Difficulty State based on the Enum Progress
        foreach (var header in headers)
        {
            if (header.name == "EasyHeader")
            {
                if (header.transform.childCount > 1) Destroy(header.transform.GetChild(1).gameObject); // Check if there is an existing indicator and delete it
                Instantiate(progressIndicators[(int)_easy], header.transform); // Add new state based on prefab
            }
            else if (header.name == "MediumHeader")
            {
                if (header.transform.childCount > 1) Destroy(header.transform.GetChild(1).gameObject); // Check if there is an existing indicator and delete it
                Instantiate(progressIndicators[(int)_medium], header.transform); // Add new state based on prefab
            }
            else if (header.name == "DifficultHeader")
            {
                if (header.transform.childCount > 1) Destroy(header.transform.GetChild(1).gameObject); // Check if there is an existing indicator and delete it
                Instantiate(progressIndicators[(int)_difficult], header.transform); // Add new state based on prefab
            }
        }

    }

    private void UpdateProgressEnums(int easy, int medium, int difficult)
    {
        _easy = (Progress)easy;
        _medium = (Progress)medium;
        _difficult = (Progress)difficult;
    }

    enum Progress
    {
        InProgress, // 0 - Index
        Completed, // 1 - Index
        Locked // 2 - Index
    }
    enum CurrentCourse
    {
        c,
        python,
        java
    }
}
