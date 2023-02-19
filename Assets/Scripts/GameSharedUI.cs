using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSharedUI : MonoBehaviour
{

    #region Singleton class: GameSharedUI

    public static GameSharedUI Instance;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] TMP_Text[] moneyUITxt;

    public Button[] lessonBtn;


    int reachedLesson;

    void Start()
    {

        UpdateMoneyUITxt();
    }

    void Update()
    {
        this.reachedLesson = DataManager.getReachedLesson();

    }

    public void UpdateMoneyUITxt()
    {
        for (int x = 0; x < moneyUITxt.Length; x++)
        {
            SetMoneyTxt(moneyUITxt[x], DataManager.GetMoney());
        }
    }

    void SetMoneyTxt(TMP_Text textMesh, int value)
    {
        textMesh.text = value.ToString();
    }

    public void updateButtonDisabled()
    {
        foreach (Button btn in lessonBtn)
        {
            btn.interactable = false;
        }

        if (lessonBtn.Length >= reachedLesson)
        {
            for (int i = 0; i < reachedLesson; i++)
            {
                lessonBtn[i].interactable = true;
            }
        }
        else
        {
            Debug.Log("No Levels Left");
        }
    }
}
