using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUISidePanel : MonoBehaviour
{
    [Header ("Information")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subTitle;
    [SerializeField] private GameObject requirement;

    [Header ("Rewards")]
    [SerializeField] private GameObject rewards;
    [SerializeField] private TextMeshProUGUI amount;


}
