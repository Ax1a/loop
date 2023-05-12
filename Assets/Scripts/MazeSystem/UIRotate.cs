using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIRotate : MonoBehaviour
{
    [SerializeField] float duration = 5f;
    [SerializeField] Ease showEase = Ease.Linear;
    void Start()
    {
        transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), duration, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(showEase);
    }
}
