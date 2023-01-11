using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject QuestPanel;
    [SerializeField] TextMeshProUGUI Title;

    [Header("Animation")]
    [SerializeField] private float _cycleLength = 2;
    [SerializeField] private float _xCoord = 130f;

    [Header("Tutorial List")]
    [SerializeField] private List<string> tutorialMissions;
    void Start()
    {
        QuestPanel.SetActive(true);
        QuestPanel.transform.DOMoveX(_xCoord, _cycleLength).SetEase(Ease.InOutSine);
    }
}
