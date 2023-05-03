using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header ("Objects")]
    public TextMeshProUGUI[] UI_TIME_TEXT;
    public TextMeshProUGUI[] UI_DATE_TEXT;
    [SerializeField] private GameObject currentDayPopup;
    public TimeFormat timeFormat = TimeFormat.Hour_24;
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;

    [Header ("Params")]
    public float secPerMin = 1;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;
    [SerializeField] private float maxSunLightIntensity;
    [SerializeField] private float maxMoonLightIntensity;
    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;
    
    #region Private Hidden Variables
    private string _time;
    private string _date;
    private bool isAm = false;
    private TextMeshProUGUI currentDayText;
    private int hr;
    private int min;
    private int day;
    int maxHr = 24;
    int maxMin = 60;
    float timer = 0;
    #endregion

    public enum TimeFormat
    {
        Hour_24,
        Hour_12
    }

    private void Awake()
    {
        hr = DataManager.GetHour();
        min = DataManager.GetMinute();
        day = DataManager.GetDay();

        if (hr < 12)
        {
            isAm = true;
        }
        else {
            isAm = false;
        }

        SetTimeDataString();
    }
    
    void Update()
    {
        if (BotGuide.Instance.guideIsActive()) return;

        if (timer >= secPerMin)
        {
            min++;

            if (min >= maxMin)
            {
                min = 0;
                hr++;

                if (hr > 1 && hr < 7) {
                    UpdateTime(Color.red);
                }
                else {
                    UpdateTime(Color.white);
                }

                if (hr < 12)
                {
                    isAm = true;
                }
                else {
                    isAm = false;
                }

                if(hr >= maxHr) 
                {
                    hr = 0;
                    day++;
                }
            }

            SetTimeDataString();

            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    
    public void NextDay() {
        isAm = true;
        min = 0;
        hr = 7;
        day += 1;

        currentDayPopup.SetActive(true);
        currentDayText.text = "Day " + day;
        currentDayPopup.GetComponent<CanvasGroup>().alpha = 1;
    }
    
    public void AddHour(int hr) {
        this.hr += hr;

        if (this.hr < 12)
        {
            isAm = true;
        }
        else {
            isAm = false;
        }

        if(hr >= maxHr) 
        {
            hr = 0;
            day++;
        }

        SetTimeDataString();
    }

    void SetTimeDataString()
    {
        switch(timeFormat)
        {
            case TimeFormat.Hour_12:
                {
                    int h;

                    if (hr >= 13)
                    {
                        h = hr - 12;
                    }
                    else if (hr == 0)
                    {
                        h = 12;
                    }
                    else
                    {
                        h = hr;
                    }

                    _time = h + ":";

                    if (min <= 9)
                    {
                        _time += "0" + min;
                    }
                    else
                    {
                        _time += min;
                    }

                    if (isAm)
                    {
                        _time += " AM";
                    }
                    else
                    {
                        _time += " PM";
                    }

                    break;
                }
            case TimeFormat.Hour_24:
                {
                    if (hr <= 9)
                    {
                        _time = "0" + hr + ":";
                    }
                    else
                    {
                        _time = hr + ":";
                    }

                    if (min <= 9)
                    {
                        _time += "0" + min;
                    }
                    else
                    {
                        _time += min;
                    }
                    break;
                }
        }

        UpdateTime(Color.white);
        UpdateDate();

        if (currentDayText == null) currentDayText = currentDayPopup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentDayText.text = "Day " + day;
    }

    private void UpdateTime(Color color) {
        foreach (var time in UI_TIME_TEXT)
        {
            time.text = _time;
            time.color = color;
        }
    }

    private void UpdateDate() {
        for (int i = 0; i < UI_DATE_TEXT.Length; i++)
        {
            UI_DATE_TEXT[i].text = "Day " + day;
        }
    }

    public int Minute
    {
        get { return min; }
        set
        {
            min = value;
        }
    }

    public int Hour
    {
        get { return hr; }
        set
        {
            hr = value;
        }
    }

    public int Day
    {
        get { return day; }
        set
        {
            day = value;
        }
    }
}
