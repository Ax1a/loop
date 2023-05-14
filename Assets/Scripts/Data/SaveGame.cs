using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    [SerializeField] private Clock timeAndDate;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject savingAnimation;

    public static SaveGame Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(300);
            SaveGameState();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SaveGameState() {
        StartCoroutine(SavingAnimation());
        
        DataManager.SetPlayerCoord(player.transform.position);
        DataManager.SetDay(timeAndDate.Day);
        DataManager.SetHour(timeAndDate.Hour);
        DataManager.SetMinute(timeAndDate.Minute);
        
        DataManager.SavePlayerData();
    }

    IEnumerator SavingAnimation() {
        savingAnimation.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        savingAnimation.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DataManager.LoadPlayerData();
    }
}
