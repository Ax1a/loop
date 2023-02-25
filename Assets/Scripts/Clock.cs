using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI[] UI_TIME_TEXT;
    public TextMeshProUGUI[] UI_DATE_TEXT;
    public TimeFormat timeFormat = TimeFormat.Hour_24;
    public DateFormat dateFormat = DateFormat.MM_DD_YYYY;
    public float secPerMin = 1;
    
    private string _time;
    private string _date;
    
    private bool isAm = false;

    [Header("Skybox")]
    [SerializeField] private Material DaySkybox;
    [SerializeField] private Material NightSkybox;

    private int hr;
    private int min;

    private int day;
    private int month;
    private int year;

    int maxHr = 24;
    int maxMin = 60;

    int maxDay = 30;
    int maxMonth = 12;

    float timer = 0;

    public enum TimeFormat
    {
        Hour_24,
        Hour_12
    }

    public enum DateFormat
    {
        MM_DD_YYYY,
        DD_MM_YYYY,
        YYYY_MM_DD,
        YYYY_DD_MM
    }

    private void Awake()
    {
        hr = DataManager.GetHour();
        min = DataManager.GetMinute();
        day = DataManager.GetDay();
        month = DataManager.GetMonth();
        year = DataManager.GetYear();
        SetTimeDataString();

        if (hr < 12)
        {
            isAm = true;
        }
        else {
            isAm = false;
        }
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

                    if (day >= maxDay)
                    {
                        day = 1;
                        month++;

                        if (month >= maxMonth)
                        {
                            month = 1;
                            year++;
                        }
                    }
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
        min = 0;
        hr = 7;
        day += 1;
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

        switch (dateFormat)
        {
            case DateFormat.DD_MM_YYYY:
                {
                    _date = day + "/" + month + "/" + year;
                    break;
                }
            case DateFormat.MM_DD_YYYY:
                {
                    _date = month + "/" + day + "/" + year;
                    break;
                }
            case DateFormat.YYYY_DD_MM:
                {
                    _date = year + "/" + day + "/" + month;
                    break;
                }
            case DateFormat.YYYY_MM_DD:
                {
                    _date = year + "/" + month + "/" + day;
                    break;
                }
        }

        for (int i = 0; i < UI_TIME_TEXT.Length; i++)
        {
            UI_TIME_TEXT[i].text = _time;
        }

        for (int i = 0; i < UI_DATE_TEXT.Length; i++)
        {
            UI_DATE_TEXT[i].text = _date;
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

    public int Month
    {
        get { return month; }
        set
        {
            month = value;
        }
    }
}
