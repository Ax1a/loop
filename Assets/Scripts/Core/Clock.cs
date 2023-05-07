using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject currentDayPopup;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Light sunLightEffect;
    [SerializeField] private GameObject godRay;
    [SerializeField] private GameObject passedOutPopup;
    public TextMeshProUGUI medicalExpensesTxt;
    public TextMeshProUGUI[] UI_TIME_TEXT;
    public TextMeshProUGUI[] UI_DATE_TEXT;
    public TextMeshProUGUI[] UI_WEEK_TEXT;

    [Header("Params")]
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
    int passHr = 2;
    int passMin = 0;
    int maxHr = 24;
    int maxMin = 60;
    private int dayOfWeek = 1;
    float timer = 0;
    string currentBgMusic = "";
    #endregion
    [HideInInspector] public Weekdays weekDay;

    public enum TimeFormat
    {
        Hour_24,
        Hour_12
    }

    public enum Weekdays 
    {
        Mon,
        Tue,
        Wed,
        Thu,
        Fri,
        Sat,
        Sun
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
        else
        {
            isAm = false;
        }

        dayOfWeek = (day-1) % 7;
        SetTimeDataString();
    }

    private void Start()
    {
        UpdateLightRotation();
        UpdateLightSettings();
    }

    void Update()
    {
        if (BotGuide.Instance.guideIsActive()) return;

        if (timer >= secPerMin)
        {
            min++;

            // To be updated // generate random time from 2 - 5
            if (hr == passHr && min == passMin && generatedTime)
            {
                StartCoroutine(PassedOut());
            }
            if (min >= maxMin)
            {
                min = 0;
                hr++;

                // Generate random time on 2 am
                if (hr == 2)
                {
                    GenerateRandomTime();
                }

                // Used for formatting time
                if (hr < 12)
                {
                    isAm = true;
                }
                else
                {
                    isAm = false;
                }

                // Reset hour and add day
                if (hr >= maxHr)
                {
                    hr = 0;
                    day++;
                    dayOfWeek = (day-1) % 7;
                }
            }

            if (!UIController.Instance.OtherPanelActive())
            {
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

    private void GenerateRandomTime()
    {
        generatedTime = true;
        // Generate random time;
        passHr = UnityEngine.Random.Range(2, 5);
        passMin = UnityEngine.Random.Range(0, 60);
    }

    private IEnumerator PassedOut() {
        int expenseCost = UnityEngine.Random.Range(50, 250);

        passedOutPopup.SetActive(true);
        medicalExpensesTxt.text = expenseCost.ToString();
        DataManager.SpendMoney(expenseCost);
        yield return new WaitForSeconds(3.5f);
        passedOutPopup.SetActive(false);
        NextDay();
        Energy.Instance.SetCurrentEnergy(8);
        generatedTime = false;
    }

    public void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(directionalLight.transform.forward, Vector3.down);
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
        directionalLight.colorTemperature = Mathf.Lerp(12707, 3869, lightChangeCurve.Evaluate(dotProduct));

        // Show or hide the god rays on the windows
        if (hr >= 7 && hr <= 11)
        {
            godRay.SetActive(true);
        }
        else
        {
            godRay.SetActive(false);
        }

        // Set the sun light intensity on the window
        if (hr >= 6 && hr <= 14)
        {
            sunLightEffect.intensity = 134;
        }
        else
        {
            if (sunLightEffect.intensity > 0) sunLightEffect.intensity -= 5;
        }

        // Check if it is night and change the background music accordingly
        string bgMusic = "";
        if (hr >= 19 || hr < 7)
        {
            bgMusic = "Night";
        }
        else
        {
            bgMusic = "Day";
        }

        // Only play new background music if it's different from the current one
        if (bgMusic != currentBgMusic)
        {
            currentBgMusic = bgMusic;
            AudioManager.Instance.PlayMusic(bgMusic);
        }
    }

    public void NextDay()
    {
        generatedTime = false;
        if(!(hr >= 0 && hr <= 6)) day += 1;
        isAm = true;
        min = 0;
        hr = 7;
        dayOfWeek = (day-1) % 7;

        currentDayPopup.SetActive(true);
        currentDayText.text = day.ToString();
        currentDayPopup.GetComponent<CanvasGroup>().alpha = 1;

        UpdateLightRotation();
        UpdateLightSettings();
        SetTimeDataString();
    }

    public void UpdateLightRotation()
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
    
    public void SetHour(int hr) {
        this.hr = hr;

        if (this.hr < 12)
        {
            isAm = true;
        }
        else
        {
            isAm = false;
        }

        if (hr >= maxHr)
        {
            hr = 0;
            day++;
        }

        UpdateLightRotation();
        UpdateLightSettings();
        SetTimeDataString();
    }

    void SetTimeDataString()
    {
        switch (timeFormat)
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

        if (hr > 1 && hr < 7)
        {
            UpdateTime(Color.red);
        }
        else
        {
            UpdateTime(Color.white);
        }
        UpdateWeekday();
        UpdateDate();

        if (currentDayText == null) currentDayText = currentDayPopup.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentDayText.text = "Day " + day.ToString();
    }

    private void UpdateTime(Color color)
    {
        foreach (var time in UI_TIME_TEXT)
        {
            time.text = _time;
            time.color = color;
        }
    }

    private void UpdateDate()
    {
        for (int i = 0; i < UI_DATE_TEXT.Length; i++)
        {
            UI_DATE_TEXT[i].text = day.ToString();
            LayoutRefresher.Instance.RefreshContentFitter((RectTransform)UI_DATE_TEXT[i].transform);
        }
    }

    private void UpdateWeekday() {
        weekDay = (Weekdays)dayOfWeek;
        for (int i = 0; i < UI_WEEK_TEXT.Length; i++)
        {
            UI_WEEK_TEXT[i].text = weekDay + ". ";
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
