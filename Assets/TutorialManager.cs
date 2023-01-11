using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] QuestPanel;
    [SerializeField] TextMeshProUGUI Title;

    [Header("Animation")]
    [SerializeField] private float _cycleLength = 1;
    [SerializeField] private float _xCoord = 572f;
    [SerializeField] private float animationDelay = 1f;

    [Header("Tutorial List")]
    [SerializeField] private List<string> tutorialMissions;
    void Start()
    {
        // StartCoroutine(DelayAnimation(animationDelay));
    }

    IEnumerator DelayAnimation(float delayTime)
    {

        foreach (var quest in QuestPanel)
        {
            quest.SetActive(true);
            quest.transform.DOMoveX(_xCoord, _cycleLength).SetEase(Ease.InOutSine);
            yield return new WaitForSeconds(delayTime);
        }
    }
}
