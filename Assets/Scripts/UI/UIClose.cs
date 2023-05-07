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
            Time.timeScale = 1;
            StartCoroutine(_animationClose());
        }
    }

    private IEnumerator _animationClose() {
        base.FadeOut();
        if (mainUI != null) mainUI.GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(base.getAnimationDuration());
        gameObject.SetActive(false);
        if (indicatorCanvas != null) {
            indicatorCanvas.SetActive(true);
            UIController.Instance.onTopCanvas.SetActive(true);
        }
        else {
            Debug.Log("Indicator Canvas is null");
        }
        UIController.Instance.SetPanelActive(false);
        UIController.Instance.gameUIActive = false;
    }

    // For button functions
    public void AnimationClose() {
        StartCoroutine(_animationClose());
    }
}
