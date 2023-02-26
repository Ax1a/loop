using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] TMP_Text[] playerNameTxts;

    private void Awake() {
        foreach (var name in playerNameTxts)
        {
            name.text = DataManager.GetPlayerName();    
        }
        // PlayerName.text = DataManager.GetPlayerName();
    }
}
