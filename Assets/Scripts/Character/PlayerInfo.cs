using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] TMP_Text[] playerNameTxts;
    [SerializeField] TMP_Text[] playerLevel;
    [SerializeField] Image expBarFill;
    public static PlayerInfo Instance;
    private int _requiredExp;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        foreach (var name in playerNameTxts)
        {
            name.text = DataManager.GetPlayerName();    
        }

        foreach (var levelTxt in playerLevel)
        {
            levelTxt.text = DataManager.GetPlayerLevel().ToString();
        }

        _requiredExp = GetRequiredExperienceForLevel(DataManager.GetPlayerLevel());
        UpdateLevelBar();
    }

    private void Update() {
        if (DataManager.GetExp() >= _requiredExp) {
            Debug.Log("Level Up!");
        }
    }

    private int GetRequiredExperienceForLevel(int level) {
        return (int) (500 * Mathf.Pow(1.2f, level));
    }

    private void LevelUp() {

    }

    private void UpdateLevelBar() {
        foreach (var levelTxt in playerLevel)
        {
            levelTxt.text = DataManager.GetPlayerLevel().ToString();
        }

        expBarFill.fillAmount = (float) DataManager.GetExp() / (float) _requiredExp;
    }
}
