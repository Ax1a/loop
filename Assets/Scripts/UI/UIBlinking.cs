using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIBlinking : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float minVal = 0f;
    [SerializeField] private float maxVal = 1f;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private int loopCount = -1;
    private Sequence sequence;
    private TextMeshProUGUI animatedText;

    private void OnEnable() {
        sequence = DOTween.Sequence();
        animatedText = gameObject.GetComponent<TextMeshProUGUI>();

        playAnimation();
    }

    private void playAnimation() {
        // Add a fade-out tween to the sequence
        sequence.Append(animatedText.DOFade(minVal, animationDuration));

        // Add a fade-in tween to the sequence
        sequence.Append(animatedText.DOFade(maxVal, animationDuration));

        // Set the number of loops to be infinite
        sequence.SetLoops(loopCount);
    }
}
