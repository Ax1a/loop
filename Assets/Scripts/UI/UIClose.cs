using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : UIFade
{
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject indicatorCanvas;

    void Update()
    {
        if(Input.GetKeyDown(InputManager.Instance.exit)){
            StartCoroutine(_animationClose());
        }
    }

    private IEnumerator _animationClose() {
        base.FadeOut();
        mainUI.GetComponent<CanvasGroup>().alpha = 1;
        yield return new WaitForSeconds(base.getAnimationDuration());
        gameObject.SetActive(false);
        indicatorCanvas.SetActive(true);
        UIController.Instance.SetPanelActive(false);
    }

    // For button functions
    public void AnimationClose() {
        StartCoroutine(_animationClose());
    }
}
