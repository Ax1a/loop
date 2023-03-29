using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Energy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] energyTxts;
    [SerializeField] TextMeshProUGUI timeTxt;
    [SerializeField] Slider energyBar;
    public int maxEnergy = 10;
    private int currEnergy;
    [SerializeField] private int restoreTime = 5;
    private DateTime nextEnergyRestore;
    private DateTime lastEnergyRestore;
    private bool isRestoring = false;

    public static Energy Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey("currEnergy"))
        {
            PlayerPrefs.SetInt("currEnergy", 10);
            Load();
            UpdateEnergy();
            StartCoroutine(RestoreEnergy());
        }
        else
        {
            Load();
            UpdateEnergy();
            StartCoroutine(RestoreEnergy());
        }
    }

    // Getters
    public int GetCurrentEnergy() {
        return currEnergy;
    }

    // Setters
    public void SetCurrentEnergy(int energy) {
        currEnergy = energy;
        UpdateEnergy();
    }

    public void ResetEnergy() {
        if(currEnergy >= 1)
        {
            currEnergy = Math.Min(maxEnergy, currEnergy + (maxEnergy - currEnergy));
            UpdateEnergy();
            UpdateEnergyTime();
        }
    }

    //on click, use energy.
    public void UseEnergy(int energyCost)
    {
        if(currEnergy >= 1)
        {
            currEnergy -= energyCost;
            UpdateEnergy();

            if(isRestoring == false)
            {
                if(currEnergy + 1 == maxEnergy)
                {
                    nextEnergyRestore = AddDuration(DateTime.Now, restoreTime);
                }

                StartCoroutine(RestoreEnergy());
            }
            Debug.Log("Energy Used. Current Energy:"+currEnergy);
        }
        else
        {
            Debug.Log("No energy");
        }
    }

    //add energy
    public void AddEnergy()
    {
        if(currEnergy >= 1)
        {
            currEnergy = Math.Min(maxEnergy, currEnergy + 1);
            UpdateEnergy();

            if(isRestoring == false)
            {
                if(currEnergy + 1 == maxEnergy)
                {
                    nextEnergyRestore = AddDuration(DateTime.Now, restoreTime);
                }

                StartCoroutine(RestoreEnergy());
            }
        }
    }


    //parsing 
    private DateTime StringToDate(string datetime)
    {
        if(String.IsNullOrEmpty(datetime))
        {
            return DateTime.Now;
        }
        else
        {
            return DateTime.Parse(datetime);
        }
    }

    //load last energy since game
    private void Load()
    {
        currEnergy = PlayerPrefs.GetInt("currEnergy");
        nextEnergyRestore = StringToDate(PlayerPrefs.GetString("nextEnergyRestore"));
        lastEnergyRestore = StringToDate(PlayerPrefs.GetString("lastEnergyRestore"));
    }

    //save last energy since game
    private void Save()
    {
        PlayerPrefs.SetInt("currEnergy", currEnergy);
        PlayerPrefs.SetString("nextEnergyRestore", nextEnergyRestore.ToString());
        PlayerPrefs.SetString("lastEnergyRestore", lastEnergyRestore.ToString());    
    }

    //update energy 
    private void UpdateEnergy()
    {
        foreach (var txt in energyTxts)
        {
            txt.text = currEnergy.ToString() + "/" + maxEnergy.ToString();
        }
        energyBar.maxValue = maxEnergy;
        energyBar.value = currEnergy;
    }

    //update energy timer 
    public void UpdateEnergyTime()
    {
        if(currEnergy >= maxEnergy)
        {
            timeTxt.text = "Full";
            return;
        }

        TimeSpan time = nextEnergyRestore - DateTime.Now;
        string timeValue = String.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
        timeTxt.text = timeValue;
    }

    //add duration to energy timer
    private DateTime AddDuration(DateTime datetime, int duration)
    {
        return datetime.AddSeconds(duration);
        //or AddMinutes
    }

    //timer checks
    private IEnumerator RestoreEnergy()
    {
        UpdateEnergyTime();
        isRestoring = true;

        while(currEnergy < maxEnergy)
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime nextDateTime = nextEnergyRestore;
            bool isEnergyAdding = false;

            while(currentDateTime >= nextDateTime)
            {
                if(currEnergy < maxEnergy)
                {
                    isEnergyAdding = true;
                    currEnergy++;
                    UpdateEnergy();
                    DateTime timeToAdd = lastEnergyRestore > nextDateTime ? lastEnergyRestore : nextDateTime;
                    nextDateTime = AddDuration(timeToAdd, restoreTime);
                }
                else
                {
                    break;
                }
            }

            if(isEnergyAdding == true)
            {
                lastEnergyRestore = DateTime.Now;
                nextEnergyRestore = nextDateTime;
            }

            UpdateEnergyTime();
            UpdateEnergy();
            Save();
            yield return null;
        }

        isRestoring = false;
    }

}
