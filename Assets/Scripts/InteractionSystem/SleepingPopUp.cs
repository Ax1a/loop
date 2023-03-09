using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SleepingPopUp : MonoBehaviour
{
    Clock _clock;
    UIFade uiFade;
    [SerializeField] private GameObject Time;
    [SerializeField] private Image sleepingIndicator;
    [SerializeField] private TextMeshProUGUI timeText;
    private int _currentHr, _currentMin, _hr;

    public static SleepingPopUp Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public IEnumerator ShowPopUp() {
        _clock = Time.GetComponent<Clock>();
        uiFade = sleepingIndicator.GetComponent<UIFade>();

        sleepingIndicator.gameObject.SetActive(true);
        _currentHr = _clock.Hour;
        _currentMin = _clock.Minute;
        UIController.Instance.SetPanelActive(true);
        
        while (true) {
            if (_currentHr == 7 && _currentMin == 0) {
                _hr = 7;
                SetDisplayTime();
                break;
            }

            SetTimeFormat();

            yield return new WaitForSeconds(0.03f);
        }

        uiFade.FadeOut();

        _clock.NextDay();
        yield return new WaitForSeconds(uiFade.getAnimationDuration());
        sleepingIndicator.gameObject.SetActive(false);
        UIController.Instance.SetPanelActive(false);
        SaveGame.Instance.SaveGameState();
    }

    private void SetTimeFormat() {
        if (_currentHr > 12) {
            _hr = _currentHr - 12;
        }
        else if (_currentHr == 0) {
            _hr = 12;
        } 
        else {
            _hr = _currentHr;
        }

        _currentMin += Random.Range(1, Mathf.Min(60 - _currentMin, 11));

        if (_currentMin >= 60)
        {
            _currentMin = 0;

            // Increment the hours and reset to 0 if it reaches 24
            _currentHr++;
            if (_currentHr >= 24)
            {
                _currentHr = 0;
            }
        }

        SetDisplayTime();
    }

    private void SetDisplayTime() {
        timeText.text = _hr.ToString("00") + ":" + _currentMin.ToString("00") + " ";

        if (_currentHr < 12) {
            timeText.text += "AM";
        }
        else {
            timeText.text += "PM";
        }
    }
}
