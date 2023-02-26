using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSharedUI : MonoBehaviour
{

    #region Singleton class: GameSharedUI

    public static GameSharedUI Instance;

    public Button[] lessonBtn;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    [SerializeField] TMP_Text[] moneyUITxt;

    void Start()
    {

        UpdateMoneyUITxt();
   
    }

    void Update()
    {

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

}
