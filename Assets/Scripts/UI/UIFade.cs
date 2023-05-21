using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIFade : MonoBehaviour
{
		/* 
    * Attach to a UI and customize the params
    *	Add CanvasGroup to the UI Gameobject and attach it to the script
    */
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationDuration;
		[SerializeField] private float animationDelay = 0;
		[SerializeField] private bool isFadeOut = false;
		[SerializeField] private bool autoDisableObj = false;

    private Tween fadeTween;

    private void OnEnable() {
				if (isFadeOut) {
					FadeOut();
				}
				else {
					FadeIn();
				}
    }

		private void OnDisable() {
			canvasGroup.alpha = 0;
		}

    public float getAnimationDuration() {
        return animationDuration;
    }

    public void FadeOut()
		{
				Fade(0f, animationDuration, () =>
				{
						canvasGroup.interactable = false;
						canvasGroup.blocksRaycasts = false;
						if (autoDisableObj) gameObject.SetActive(false);
				});

		}

		private void FadeIn() {
			Fade(1f, animationDuration, () =>
				{
						canvasGroup.interactable = true;
						canvasGroup.blocksRaycasts = true;
				});
		}

    private void Fade(float endValue, float duration, TweenCallback onEnd)
		{
				if (fadeTween != null)
				{
						fadeTween.Kill(false);
				}

				fadeTween = canvasGroup.DOFade(endValue, duration).SetDelay(animationDelay);
				fadeTween.onComplete += onEnd;
		}
}
