[System.Serializable] public class PlayerData
{
    public int money = 0;
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
        UnityEngine.Debug.Log("Player Data Loaded");
    }


}
