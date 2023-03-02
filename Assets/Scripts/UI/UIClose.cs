using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : UIFade
{
    [SerializeField] GameObject mainUI;

    void Update()
    {
        if(Input.GetKeyDown(InputManager.Instance.exit)){
            StartCoroutine(_animationClose());
        }
    }

    private IEnumerator _animationClose() {
        base.FadeOut();
        mainUI.SetActive(true);
        yield return new WaitForSeconds(base.getAnimationDuration());
        gameObject.SetActive(false);
        UIController.Instance.SetPanelActive(false);
    }

    // For button functions
    public void AnimationClose() {
        StartCoroutine(_animationClose());
    }
}
