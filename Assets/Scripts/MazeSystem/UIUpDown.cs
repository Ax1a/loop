using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIUpDown : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private Ease easeType = Ease.InOutQuad;
    private bool isMovingUp = true;

    void Start()
    {
        transform.DOMoveY(transform.position.y + distance, duration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
    }
}
