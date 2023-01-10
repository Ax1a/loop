using UnityEngine;
[System.Serializable] public class PlayerData
{
    
    public int money = 0;
    public Vector3 playerPos = new Vector3(-2.98900008f,6.1f,-4.40799999f);
    public int hr = 7, min = 0, day = 1, month = 1, year = 2022;
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

    public static Vector3 GetPlayerCoord() {
        return playerData.playerPos;
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
