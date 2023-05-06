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
    private int _reachedLesson;
    private int _level;
    [HideInInspector] public string course;

    void Start()
    {
        GetCurrentCourse();
        UpdateLessonState();

    }

    void Update()
    {
        if (_reachedLesson != DataManager.GetProgrammingLanguageProgress(course))
        {
            _reachedLesson = DataManager.GetProgrammingLanguageProgress(course);
            UpdateLessonState();
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
    
    //function for adding level
    public void addReachedLesson()
    {
        DataManager.AddProgrammingLanguageProgress(course);

        _reachedLesson++;

        if (DataManager.GetProgrammingLanguageProgress(course) >= easyLevelCount + mediumLevelCount + difficultLevelCount) {
            UnlockOtherCourses();
        }
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

        if (lessonBtn.Length >= _reachedLesson)
        {
            for (int i = 0; i < _reachedLesson; i++)
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
                image.gameObject.SetActive(false);
            }
            Debug.Log("No Levels Left");
        }
    }

    private void UnlockOtherCourses() {
        foreach (var item in System.Enum.GetNames (typeof(CurrentCourse)))
        {
            if (item.ToLower() == "c" && item != currentCourse.ToString()) {
                if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                    DataManager.SetProgrammingLanguageProgress(item, 0);
                }
            }
            else if (item.ToLower() == "java" && item != currentCourse.ToString()) {
                if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                    DataManager.SetProgrammingLanguageProgress(item, 0);
                }

            }
            else if (item.ToLower() == "python" && item != currentCourse.ToString()) {
                if (DataManager.GetProgrammingLanguageProgress(item) == -1) {
                    DataManager.SetProgrammingLanguageProgress(item, 0);
                }
            }
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
