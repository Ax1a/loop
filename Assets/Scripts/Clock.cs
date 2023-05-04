using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField] private GameObject currentDayPopup;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Light sunLightEffect;
    [SerializeField] private GameObject godRay;
    [SerializeField] private GameObject passedOutPopup;
    public TextMeshProUGUI[] UI_TIME_TEXT;
    public TextMeshProUGUI[] UI_DATE_TEXT;

    [Header ("Params")]
    public TimeFormat timeFormat = TimeFormat.Hour_24;
    public float secPerMin = 1;
    [SerializeField] private float rotationPerHr;
    [SerializeField] private float rotationPerMin;
    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private AnimationCurve lightChangeCurve;
    
    #region Private Hidden Variables
    private string _time;
    private string _date;
    private bool generatedTime = false;
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

    private void Start() {
        directionalLight.transform.rotation = Quaternion.Euler(120,0,0);
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

                // Show or hide the god rays on the windows
                if (hr >= 7 && hr <= 11) {
                    godRay.SetActive(true);
                }
                else {
                    godRay.SetActive(false);
                }

                // Set the sun light intensity on the window
                if (hr >= 7 && hr <= 12) {
                    sunLightEffect.intensity = 134;
                }
                else {
                    if (sunLightEffect.intensity > 0) sunLightEffect.intensity -= 35;
                }

                // To be updated // generate random time from 2 - 5
                if (hr == 2 && !generatedTime) {
                    StartCoroutine(PassedOut());
                }

                // Used for formatting time
                if (hr < 12)
                {
                    isAm = true;
                }
                else {
                    isAm = false;
                }

                // Reset hour and add day
                if(hr >= maxHr) 
                {
                    hr = 0;
                    day++;
                
                }
            }

            if (!UIController.Instance.OtherPanelActive()) {
                UpdateLightRotation();
                UpdateLightSettings();
            }
            SetTimeDataString();

            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private IEnumerator PassedOut() {
        generatedTime = true;
        passedOutPopup.SetActive(true);
        DataManager.SpendMoney(200);
        yield return new WaitForSeconds(3.5f);
        passedOutPopup.SetActive(false);
        NextDay();
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(directionalLight.transform.forward, Vector3.down);
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
        directionalLight.colorTemperature = Mathf.Lerp(12707, 3869, lightChangeCurve.Evaluate(dotProduct));
    }
    
    public void NextDay() {
        generatedTime = false;
        isAm = true;
        min = 0;
        hr = 7;
        day += 1;

        currentDayPopup.SetActive(true);
        currentDayText.text = "Day " + day;
        currentDayPopup.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void UpdateLightRotation()
    {
        float totalRotation = (hr * rotationPerHr) + (min * rotationPerMin) + 86.95f;

        if ((hr >= 0 && hr < 7) || hr >= 19)
        {
            directionalLight.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
            return;
        }
        else
        {
            directionalLight.transform.rotation = Quaternion.Euler(totalRotation, 0f, 0f);
        }
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

        if (hr > 1 && hr < 7) {
            UpdateTime(Color.red);
        }
        else {
            UpdateTime(Color.white);
        }
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
