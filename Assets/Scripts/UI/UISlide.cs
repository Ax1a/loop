using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UISlide : MonoBehaviour
{
    /* 
    * Attach to a UI and customize the params
    * tweenGO (The object that the script attached to)
    */

    [SerializeField] float showTime = 0.3f, position = 500f;
    [SerializeField] float defaultPosX = -800f, defaultPosY = 0f, delayClose = 0;
    [SerializeField] bool autoClose = false;
    [SerializeField] Ease showEase = Ease.OutBack;
    [SerializeField] Transform tweenGO;
    Coroutine coroutine;

    private void OnEnable() {
        float ratioX = (float) Screen.height / 1920f ;
        if (tweenGO == null) { tweenGO = transform; }
        tweenGO.localPosition = new Vector3(defaultPosX, defaultPosY, 0);
        tweenGO.DOMoveX(ratioX*position, showTime).SetEase(showEase);

        if (autoClose) {
            if (coroutine != null) StopCoroutine(coroutine);
            coroutine = StartCoroutine(DisableObject());
        }
    }

    private IEnumerator DisableObject() {
        yield return new WaitForSeconds(delayClose);
        gameObject.SetActive(false);
    }
}
