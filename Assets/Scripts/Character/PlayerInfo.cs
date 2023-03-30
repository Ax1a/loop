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
    private int _currentExp;
    private Coroutine _expBarCoroutine;

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
        _currentExp = DataManager.GetExp();

        expBarFill.fillAmount = (float)_currentExp / _requiredExp;
    }

    private void Update() {
        // For testing
        if (Input.GetKey(KeyCode.N)) DataManager.AddExp(50);

        if (DataManager.GetExp() == _currentExp) return;

        _currentExp = DataManager.GetExp();

        if (_currentExp >= _requiredExp) {
            LevelUp();
        }
        else {
            if (_expBarCoroutine != null) StopCoroutine(UpdateLevelBar());
            _expBarCoroutine = StartCoroutine(UpdateLevelBar());
        }
    }

    private int GetRequiredExperienceForLevel(int level) {
        return (int) (300 * Mathf.Pow(1.2f, level));
    }

    private void LevelUp() {
        int _excessExp = 0;
        if (_currentExp > _requiredExp) _excessExp = _currentExp - _requiredExp;

        DataManager.AddPlayerLevel(1);
        DataManager.SetExp(_excessExp);

        if (_expBarCoroutine != null) StopCoroutine(UpdateLevelBar());
        _expBarCoroutine = StartCoroutine(UpdateLevelBar());

        _requiredExp = GetRequiredExperienceForLevel(DataManager.GetPlayerLevel());
        SaveGame.Instance.SaveGameState();

        // Add Level Up Sound here
    }

    private IEnumerator UpdateLevelBar() {
        foreach (var levelTxt in playerLevel)
        {
            levelTxt.text = DataManager.GetPlayerLevel().ToString();
        }

        float previousFillAmount = expBarFill.fillAmount;
        float targetFillAmount = (float)_currentExp / _requiredExp;
        float elapsed = 0f;
        float duration = .3f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            expBarFill.fillAmount = Mathf.Lerp(previousFillAmount, targetFillAmount, elapsed / duration);

            // Add fill up sound here
            yield return null;
        }

        expBarFill.fillAmount = targetFillAmount;
    }
}
