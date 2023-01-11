using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] TMP_Text PlayerName;

    private void Awake() {
        PlayerName.text = DataManager.GetPlayerName();
    }
}
