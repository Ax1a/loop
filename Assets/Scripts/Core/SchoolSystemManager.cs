using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SchoolSystemManager : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI timeRangeTxt;
    [SerializeField] private Clock timeAndDate;
    [SerializeField] private GameObject schoolTimeIndicator;
    [SerializeField] private int schoolTimeHr;
    [SerializeField] private GameObject gradePanel;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    public static SchoolSystemManager Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        // if ((timeAndDate.Day % 30) == 0 & !functionCalled) {
        //     ShowGrade();
        // }
        // else if ((timeAndDate.Day % 30) != 0) {
        //     functionCalled = false;
        // }

        if ((int)timeAndDate.weekDay >= 5) {
            schoolTimeIndicator.SetActive(false);
        }
        else {
            if (timeAndDate.Hour >= 7 && timeAndDate.Hour <= 13) {
                ComputeTimeDifference();
                schoolTimeIndicator.SetActive(true);
            }
            else {
                schoolTimeIndicator.SetActive(false);
            }
        }
    }

    public void ShowGrade() {
        if (DataManager.GetDaysAttended() >= 28) {
                gradeTxt.text = "A+";
                rewardTxt.text = "1000";
                DataManager.AddMoney(1000);
        }
        else if (DataManager.GetDaysAttended() >= 22 && DataManager.GetDaysAttended() <= 27) {
            gradeTxt.text = "A";
            rewardTxt.text = "900";
            DataManager.AddMoney(900);
        }
        else if (DataManager.GetDaysAttended() >= 13 && DataManager.GetDaysAttended() <= 21) {
            gradeTxt.text = "B";
            rewardTxt.text = "700";
            DataManager.AddMoney(700);
        }
        else if (DataManager.GetDaysAttended() >= 9 && DataManager.GetDaysAttended() <= 12) {
            gradeTxt.text = "C";
            rewardTxt.text = "500";
            DataManager.AddMoney(500);
        }
        else if (DataManager.GetDaysAttended() >= 4 && DataManager.GetDaysAttended() <= 8) {
            gradeTxt.text = "D";
            rewardTxt.text = "300";
            DataManager.AddMoney(300);
        }
        else if (DataManager.GetDaysAttended() >= 0 && DataManager.GetDaysAttended() <= 3) {
            rewardTxt.text = "0";
            gradeTxt.text = "E";
        }
        gradePanel.SetActive(true);
        DataManager.SetDaysAttended(0);
    }

    private void ComputeTimeDifference() {
        int hourDiff;
        if (timeAndDate.Hour <= 12) {
            hourDiff = Mathf.Abs(schoolTimeHr - timeAndDate.Hour);
        }
        else {
            hourDiff = 0;
        }
        
        if (hourDiff == 0) {
            timeSlider.value = timeSlider.maxValue;
            ColorBlock colors = timeSlider.colors;
            timeSlider.fillRect.GetComponent<Image>().color = Color.red;
        } else {
            float sliderValue = 1f - Mathf.Clamp((float)hourDiff / 5f, 0f, 1f);
            timeSlider.value = timeSlider.maxValue * sliderValue;

            if (hourDiff >= 3) {
                timeSlider.fillRect.GetComponent<Image>().color = Color.green;
            }
            else {
                timeSlider.fillRect.GetComponent<Image>().color = Color.yellow;
            }
        }
    }
}
