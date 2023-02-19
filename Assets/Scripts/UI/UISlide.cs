using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UISlide : MonoBehaviour
{
    [SerializeField] float showTime = 0.3f, hideTime = 0.2f, position = 500f;
    [SerializeField] Ease showEase = Ease.OutBack, hideEase = Ease.InBack;
    [SerializeField] Transform tweenGO;

    private void OnEnable() {
        float ratioX = (float) Screen.height / 1920f ;
        if (tweenGO == null) { tweenGO = transform; }
        tweenGO.localPosition = new Vector3(-800f, 0, 0);
        tweenGO.DOMoveX(ratioX*position, showTime).SetEase(showEase);
    }
}
