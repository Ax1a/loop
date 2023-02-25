using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPopup : MonoBehaviour
{
    [SerializeField] float showTime = 0.3f;
    [SerializeField] Ease showEase = Ease.OutBack;
    [SerializeField] Transform tweenGO;

    private void OnEnable() {
        if (tweenGO == null) { tweenGO = transform; }
        tweenGO.localScale = Vector3.zero;
        tweenGO.DOScale(1, showTime).SetEase(showEase);
    }
}
