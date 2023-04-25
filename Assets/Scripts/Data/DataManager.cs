using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    /* 
    * Add Variable here to save in the txt file
    */
    public int money = 0;
    public int exp = 0;
    public int playerLevel = 1;
    [JsonIgnore]
    public Vector3 playerPos = new Vector3(-2.98900008f, 6.064f, -4.40799999f);
    public int hr = 7, min = 0, day = 1;
    public string name;
    public int tutorialProgress = 0;
    public int reachedLesson = 1;
    public List<DrinkInventoryList> inventoryList;
    public Dictionary<string, int> programmingLanguage = new Dictionary<string, int>() {
        {"c++", -1},
        {"java", -1},
        {"python", -1}
    };

    public List<Quest> questList = new List<Quest>();
    public List<Quest> currentQuests = new List<Quest>();
    public List<InteractionQuiz> interactionQuizzes = new List<InteractionQuiz>();
}

/* 
* You can create a class like this to make a custom list
*/
[System.Serializable]
public class DrinkInventoryList
{
    public string name;
    public int energy;
    public int quantity;
}

public static class DataManager
{
    static PlayerData playerData = new PlayerData();

    /*
        Buggy if not serialized
        For testing only
    */
    static bool isSerialized = true;

    static DataManager()
    {
        LoadPlayerData();
    }


    /* 
    * Add Getters and Setters here to be used on other scripts
    */
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
        GameSharedUI.Instance.UpdateMoneyUITxt();
        Debug.Log("Added Money: " + amount);
    }

    public static bool CanSpendMoney(int amount)
    {
        return (playerData.money >= amount);
    }

    public static void SpendMoney(int amount)
    {
        playerData.money -= amount;
        AudioManager.Instance.PlaySfx("Purchase");
        GameSharedUI.Instance.UpdateMoneyUITxt();
    }

    /*
        Clock
        Getters & Setters
    */
    public static int GetHour()
    {
        return playerData.hr;
    }

    public static int GetMinute()
    {
        return playerData.min;
    }

    public static int GetDay()
    {
        return playerData.day;
    }

    public static void SetDay(int day)
    {
        playerData.day = day;
    }

    public static void SetMinute(int minute)
    {
        playerData.min = minute;
    }

    public static void SetHour(int hour)
    {
        playerData.hr = hour;
    }

    /*
        // PLAYER INFOS //
        Player Name
        Getters & Setters
    */
    public static string GetPlayerName()
    {
        return playerData.name;
    }

    public static void SetPlayerName(string name)
    {
        playerData.name = name;
        SavePlayerData();
    }

    /*
        Player Level
        Getters & Setters
    */

    public static int GetPlayerLevel() {
        return playerData.playerLevel;
    }

    public static void AddPlayerLevel(int level) {
        playerData.playerLevel += level;
    }

    /*
        Player Experience
        Getters & Setters
    */
    public static int GetExp()
    {
        return playerData.exp;
    }

    public static void AddExp(int amount)
    {
        playerData.exp += amount;
    }

    public static void SetExp(int amount) {
        playerData.exp = amount;
    }

    /*
        Player Coord
        Getters & Setters
    */
    public static Vector3 GetPlayerCoord()
    {
        return playerData.playerPos;
    }

    public static void SetPlayerCoord(Vector3 pos)
    {
        playerData.playerPos = pos;
    }

    /*
        Tutorial Progress
        Getters & Setters
    */
    public static int GetTutorialProgress()
    {
        return playerData.tutorialProgress;
    }

    public static void SetTutorialProgress(int progress)
    {
        playerData.tutorialProgress += progress;
    }

    /*
        Inventory List
        Getters & Setters
    */
    public static string GetInventoryItemName(int index)
    {
        string value = playerData.inventoryList[index].name;

        return value;
    }

    public static int GetInventoryItemQuantity(int index)
    {
        int value = playerData.inventoryList[index].quantity;

        return value;
    }

    public static int GetInventoryListCount()
    {
        if (playerData.inventoryList == null)
        {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        int count = playerData.inventoryList.Count;

        return count;
    }

    public static List<DrinkInventoryList> GetInventoryList()
    {
        return playerData.inventoryList;
    }

    public static void AddInventoryItem(ShopItemData item)
    {
        // Check if the inventory list is null
        if (playerData.inventoryList == null)
        {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        // Check if the item name already exists in the inventory list
        DrinkInventoryList existingItem = playerData.inventoryList.Find(i => i.name == item.itemName);
        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            // Add the item to the inventory list
            DrinkInventoryList drinkItem = new DrinkInventoryList();
            drinkItem.name = item.itemName;
            drinkItem.quantity = 1;
            drinkItem.energy = item.itemEnergy;
            playerData.inventoryList.Add(drinkItem);
        }
    }

    public static void ReduceItemQuantity(string name)
    {
        // Check if the inventory list is null
        if (playerData.inventoryList == null)
        {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        // Verify if the item name already exists in the inventory list
        DrinkInventoryList existingItem = playerData.inventoryList.Find(i => i.name == name);
        if (existingItem != null)
        {
            existingItem.quantity--;
        }
    }

    public static void RemoveInventoryItem(string name)
    {
        // Check if the inventory list is null
        if (playerData.inventoryList == null)
        {
            playerData.inventoryList = new List<DrinkInventoryList>();
        }

        playerData.inventoryList.RemoveAll(item => item.name == name);
    }

    /*
        Programming Language Progress
        Getters & Setters

        Usage:
        DataManager.GetProgrammingLanguageProgress("c++"); This will get the progress of the C++ Language
        DataManager.AddProgrammingLanguageProgress("c++"); This will add 1 progress to the C++ Language

        Keys:
        c++, java, python
        Note: All small caps

    */
    public static void AddProgrammingLanguageProgress(string key)
    {
        if (playerData.programmingLanguage.ContainsKey(key))
        {
            playerData.programmingLanguage[key] += 1;
            Debug.Log("Added progress to: " + key);
            ShopCourse.Instance._checkedState = false;
            SavePlayerData();
        }
    }

    public static void CompleteProgrammingLanguages()
    {
        foreach (var key in playerData.programmingLanguage.Keys.ToList())
        {
            playerData.programmingLanguage[key] = 10;
            ShopCourse.Instance._checkedState = false;
            SavePlayerData();
        }
    }

    public static int GetProgrammingLanguageProgress(string key)
    {
        int result = -1;

        // Prevents error of null
        if (playerData.programmingLanguage == null) {
            playerData.programmingLanguage = new Dictionary<string, int>();
        }

        if (playerData.programmingLanguage.ContainsKey(key))
        {
            result = playerData.programmingLanguage[key];
        }
        
        return result;
    }

    // Used to determine the first language and give user a free course
    public static bool FirstProgrammingLanguage() {
        int count = 0;

         // Prevents error of null
        if (playerData.programmingLanguage == null) {
            playerData.programmingLanguage = new Dictionary<string, int>();
        }

        foreach (var kvp in playerData.programmingLanguage)
        {
            if (kvp.Value == -1)
            {
                count++;
                if (count == 2)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static int GetUnlockedProgrammingLanguageCount() {
        int count = 0;

         // Prevents error of null
        if (playerData.programmingLanguage == null) {
            playerData.programmingLanguage = new Dictionary<string, int>();
        }

        foreach (var progLanguage in playerData.programmingLanguage)
        {
            if (progLanguage.Value >= 0)
            {
                count++;
            }
        }

        return count;
    }

    // OLD Lesson Script
    public static int getReachedLesson()
    {
        return playerData.reachedLesson;
    }

    public static void addReachedLesson()
    {
        int lesson = 1;
        playerData.reachedLesson += lesson;
        SavePlayerData();
    }

    // resetLevel() is for testing purpose only must remove after.
    public static void resetLevel()
    {
        playerData.reachedLesson = 1;
        SavePlayerData();

    }

    /*
        Quest Progress
        Getters & Setters
    */

    public static List<Quest> QuestList {
        get { return playerData.questList; }
        set { playerData.questList = value; }
    }

    public static List<Quest> CurrentQuests {
        get { return playerData.currentQuests; }
        set { playerData.currentQuests = value; }
    }

    /*
        Interaction Quiz Progress
        Getters & Setters
    */

    public static List<InteractionQuiz> GetInteractionQuizData()
    {
        return playerData.interactionQuizzes;
    }

    public static void AddInteractionQuizData(List<InteractionQuiz> info)
    {
        if (playerData.interactionQuizzes == null)
        {
            playerData.interactionQuizzes = new List<InteractionQuiz>();
        }

        // Clear old data
        playerData.interactionQuizzes.Clear();

        playerData.interactionQuizzes.AddRange(info);
        SavePlayerData();
    }

    public static void SetInteractionQuizComplete(int i)
    {
        if (playerData.interactionQuizzes == null)
        {
            playerData.interactionQuizzes = new List<InteractionQuiz>();
        }

        playerData.interactionQuizzes[i].isComplete = true;
        SavePlayerData();
    }

    public static void ActivateInteractionQuiz(int i)
    {
        if (playerData.interactionQuizzes == null)
        {
            playerData.interactionQuizzes = new List<InteractionQuiz>();
        }

        playerData.interactionQuizzes[i].isActive = true;
        SavePlayerData();
    }

    // Save and Load Functions
    public static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "playerData.txt", isSerialized);
        UnityEngine.Debug.Log("Player Data Saved");
    }

    public static void LoadPlayerData()
    {
        playerData = BinarySerializer.Load<PlayerData>("playerData.txt", isSerialized);
    }


}
