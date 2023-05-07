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


    // Update is called once per frame
    void Update()
    {
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
