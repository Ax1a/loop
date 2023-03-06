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

    private void OnEnable() {
        playAnimation();
    }

    private void playAnimation() { 
        // Arrow animation
        transform.DOPunchPosition(new Vector3(x,y,0), animationDuration, 3, 1f, true).SetLoops(loopCount);
    }
}
