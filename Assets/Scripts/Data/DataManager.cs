using UnityEngine;
using System.Collections.Generic;
[System.Serializable] public class PlayerData
{ 
    public int money = 0;
    public Vector3 playerPos = new Vector3(-2.98900008f,6.064f,-4.40799999f);
    public int hr = 7, min = 0, day = 1, month = 1, year = 2022;
    public string name;
    public int questProgress = 0;
    public int reachedLesson; 
    public List<DrinkInventoryList> inventoryList;
    public Dictionary<string, int> programmingLanguage = new Dictionary<string, int>() {
        {"c++", -1},
        {"java", -1},
        {"python", -1}
    };
    // public Dictionary<string, int> playerReachedLevels = new Dictionary<string, int>() {
    //     {"c++", 1},
    //     {"java", 1},
    //     {"python", 1}
    // };

    // public Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
}

[System.Serializable] public class DrinkInventoryList {
    public string name;
    public int energy;
    public int quantity;
}

public static class DataManager
{
    static PlayerData playerData = new PlayerData();

    static DataManager()
    {
        LoadPlayerData();
    }

    /*
        Money
        Getters & Setters
    */ 
    public static int GetMoney() 
    {
        return playerData.money;
    }

    public static void AddMoney(int amount)
    {
        playerData.money += amount;
    }

    public static bool CanSpendMoney (int amount)
    {
        return (playerData.money >= amount);
    }

    public static void SpendMoney (int amount)
    {
        playerData.money -= amount;
    }

    /*
        Clock
        Getters & Setters
    */ 
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

    public static void SetYear(int year) {
        playerData.year = year;
    }

    public static void SetMonth(int month) {
        playerData.month = month;
    }

    public static void SetDay(int day) {
        playerData.day = day;
    }

    public static void SetMinute(int minute) {
        playerData.min = minute;
    }

    public static void SetHour(int hour) {
        playerData.hr = hour;
    }

    /*
        Player Name
        Getters & Setters
    */ 
    public static string GetPlayerName() {
        return playerData.name;
    }

    public static void SetPlayerName(string name) {
        playerData.name = name;
        SavePlayerData();
    }

    /*
        Player Coord
        Getters & Setters
    */ 
    public static Vector3 GetPlayerCoord() {
        return playerData.playerPos;
    }

    public static void SetPlayerCoord(Vector3 pos) {
        playerData.playerPos = pos;
    }

    /*
        Quest Progress
        Getters & Setters
    */ 
    public static int GetQuestProgress() {
        return playerData.questProgress;
    }

    public static void SetQuestProgress(int progress) {
        playerData.questProgress += progress;
    }

    /*
        Inventory List
        Getters & Setters
    */ 
    public static string GetInventoryItemName(int index) {
        string value = playerData.inventoryList[index].name;

        return value;
    }

    public static int GetInventoryItemQuantity(int index) {
        int value = playerData.inventoryList[index].quantity;

        return value;
    }

    public static int GetInventoryListCount() {
        if (playerData.inventoryList == null) {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }
        
        int count = playerData.inventoryList.Count;

        return count;
    }

    public static List<DrinkInventoryList> GetInventoryList() {
        return playerData.inventoryList;
    }

    public static void AddInventoryItem(ShopItemData item) {
        // Check if the inventory list is null
        if (playerData.inventoryList == null) {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        // Check if the item name already exists in the inventory list
        DrinkInventoryList existingItem = playerData.inventoryList.Find(i => i.name == item.itemName);
        if (existingItem != null) {
            existingItem.quantity++;
        } else {
            // Add the item to the inventory list
            DrinkInventoryList drinkItem = new DrinkInventoryList();
            drinkItem.name = item.itemName;
            drinkItem.quantity = 1;
            drinkItem.energy = item.itemEnergy;
            playerData.inventoryList.Add(drinkItem);
        }
        SavePlayerData();
    }

    public static void ReduceItemQuantity(string name) {
        // Check if the inventory list is null
        if (playerData.inventoryList == null) {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

         // Verify if the item name already exists in the inventory list
        DrinkInventoryList existingItem = playerData.inventoryList.Find(i => i.name == name);
        if (existingItem != null) {
            existingItem.quantity--;
        }
    }

    public static void RemoveInventoryItem(string name) {
        // Check if the inventory list is null
        if (playerData.inventoryList == null) {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        playerData.inventoryList.RemoveAll(item => item.name == name);
    }

    /*
        Programming Language Progress
        Getters & Setters
    */ 
    public static void AddProgrammingLanguageProgress(string key)
    {
        if (playerData.programmingLanguage.ContainsKey(key))
        {
            playerData.programmingLanguage[key] += 1;
            SavePlayerData();
        }
    }

    public static int GetProgrammingLanguageProgress(string key)
    {
        int result = -1;
        PlayerData playerData = new PlayerData();
        if (playerData.programmingLanguage.ContainsKey(key))
        {
            result = playerData.programmingLanguage[key];
        }

        return result;
    }

    // public static void AddLevelProgress(string key)
    // {
    //     if (playerData.playerReachedLevels.ContainsKey(key))
    //     {
    //         playerData.playerReachedLevels[key] += 1;
    //         SavePlayerData();
    //     }
        
    // }
    // public static int GetLevelProgress(string key)
    // {
    //     int result = 0;
    //     PlayerData playerData = new PlayerData();
    //     if (playerData.playerReachedLevels.ContainsKey(key))
    //     {
    //         result = playerData.playerReachedLevels[key];
    //     }

    //     return result;
    // }


    public static int getReachedLesson()
    {
        return playerData.reachedLesson; 
    }

    public static void addReachedLesson ()
    {
        int lesson = 1;
        playerData.reachedLesson += lesson;
        SavePlayerData();
    }

    // resetLevel() is for testing purpose only must remove after.
    public static void resetLevel ()
    {
        playerData.reachedLesson = 1;
        SavePlayerData();

    }

    // Save and Load Functions
    public static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "playerData.txt");
        UnityEngine.Debug.Log("Player Data Saved");
    }

    public static void LoadPlayerData()
    {
        playerData = BinarySerializer.Load<PlayerData>("playerData.txt");
    }


}
