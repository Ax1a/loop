using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIPopup : MonoBehaviour
{
    /* 
    * Attach to a UI and customize the params
    * tweenGO (The object that the script attached to)
    */
    
    [SerializeField] float showTime = 0.3f;
    [SerializeField] Ease showEase = Ease.OutBack;
    [SerializeField] Transform tweenGO;
    [SerializeField] bool autoClose = false;

    private void OnEnable() {
        if (tweenGO == null) { tweenGO = transform; }
        tweenGO.localScale = Vector3.zero;
        tweenGO.DOScale(1, showTime).SetEase(showEase);

        if (autoClose == true) {
            tweenGO.DOScale(0, showTime).SetEase(showEase).SetDelay(2.5f).OnComplete(() => {
                tweenGO.gameObject.SetActive(false);
            });
        }
    }
}
