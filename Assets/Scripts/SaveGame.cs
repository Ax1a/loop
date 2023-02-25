using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private GameObject timeAndDate;
    [SerializeField] private GameObject player;
    Energy _energy;
    Clock _timeAndDate;

    public static SaveGame Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SaveGameState() {
        _timeAndDate = timeAndDate.GetComponent<Clock>();

        DataManager.SetPlayerCoord(player.transform.position);
        DataManager.SetDay(_timeAndDate.Day);
        DataManager.SetHour(_timeAndDate.Hour);
        DataManager.SetMonth(_timeAndDate.Month);
        DataManager.SetMinute(_timeAndDate.Minute);
    }
}
