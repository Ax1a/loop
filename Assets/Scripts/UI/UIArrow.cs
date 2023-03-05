using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIArrow : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float animationDuration = .5f;
    [SerializeField] private int loopCount = -1;
    [SerializeField] private float x;
    [SerializeField] private float y;
    Sequence sequence;

    private void OnEnable() {
        sequence = DOTween.Sequence();
        playAnimation();
    }

    private void playAnimation() { 
        // Add a fade-out tween to the sequence
        sequence.Append(transform.DOPunchPosition(new Vector3(x,y,0), animationDuration, 3, 1f, true));

        // Add a fade-in tween to the sequence
        // sequence.Append(transform.DOLocalMove(new Vector3(0, 0, 0), animationDuration));

        // Set the number of loops to be infinite
        sequence.SetLoops(loopCount);
    }
}
