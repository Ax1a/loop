using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameVersionDisplay : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI _Text = gameObject.GetComponent<TextMeshProUGUI>();
        _Text.text = "v" + Application.version;
    }
}
