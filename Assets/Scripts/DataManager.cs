using UnityEngine;
using System.Collections.Generic;
[System.Serializable] public class PlayerData
{ 
    public int money = 0;
    public Vector3 playerPos = new Vector3(-2.98900008f,6.1f,-4.40799999f);
    public int hr = 7, min = 0, day = 1, month = 1, year = 2022;
    public string name;
    public int questProgress = 0;
    public Dictionary<string, int> programmingLanguage = new Dictionary<string, int>() {
        {"c++", -1},
        {"java", -1},
        {"python", -1}
    };

    public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
}

public static class DataManager
{
    static PlayerData playerData = new PlayerData();

    static DataManager()
    {
        LoadPlayerData();
    }

    public static int GetMoney() 
    {
        return playerData.money;
    }

    public static int GetHour() {
        return playerData.hr;
    }

    public static int GetMinute() {
        return playerData.min;
    }

    public static int GetDay() {
        return playerData.day;
    }

    public static int GetMonth() {
        return playerData.month;
    }

    public static int GetYear() {
        return playerData.year;
    }

    public static string GetPlayerName() {
        return playerData.name;
    }
    public static Vector3 GetPlayerCoord() {
        return playerData.playerPos;
    }

    public static int GetQuestProgress() {
        return playerData.questProgress;
    }

    public static int GetProgrammingLanguage() {
        return playerData.questProgress;
    }

    public static void SetProgrammingLanguage(string key, int value)
    {
        if (playerData.programmingLanguage.ContainsKey(key))
        {
            playerData.programmingLanguage[key] = value;
        }
        else
        {
            playerData.programmingLanguage.Add(key, value);
        }
    }

    public static int GetProgrammingLanguage(string key)
    {
        int result = -1;
        PlayerData playerData = new PlayerData();
        if (playerData.programmingLanguage.ContainsKey(key))
        {
            result = playerData.programmingLanguage[key];
        }

        return result;
    }

    public static void SetQuestProgress(int progress) {
        playerData.questProgress += progress;
        SavePlayerData();
    }
    public static void SetPlayerName(string name) {
        playerData.name = name;
        SavePlayerData();
    }

    public static void SetPlayerCoord(Vector3 pos) {
        playerData.playerPos = pos;
        SavePlayerData();
    }

    public static void SetYear(int year) {
        playerData.year = year;
        SavePlayerData();
    }

    public static void SetMonth(int month) {
        playerData.month = month;
        SavePlayerData();
    }

    public static void SetDay(int day) {
        playerData.day = day;
        SavePlayerData();
    }

    public static void SetMinute(int minute) {
        playerData.min = minute;
        SavePlayerData();
    }

    public static void SetHour(int hour) {
        playerData.hr = hour;
        SavePlayerData();
    }

    public static void AddMoney(int amount)
    {
        playerData.money += amount;
        SavePlayerData();
    }

    public static bool CanSpendMoney (int amount)
    {
        return (playerData.money >= amount);
    }

    public static void SpendMoney (int amount)
    {
        playerData.money -= amount;
        SavePlayerData();
    }

    static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "playerData.txt");
        UnityEngine.Debug.Log("Player Data Saved");
    }

    static void LoadPlayerData()
    {
        playerData = BinarySerializer.Load<PlayerData>("playerData.txt");
    }


}
