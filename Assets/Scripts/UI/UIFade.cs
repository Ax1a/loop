using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationDuration;
		[SerializeField] private bool isFadeOut = false;

    private Tween fadeTween;

    private void OnEnable() {
				if (isFadeOut) {
					FadeOut();
				}
				else {
					FadeIn();
				}
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

				fadeTween = canvasGroup.DOFade(endValue, duration);
				fadeTween.onComplete += onEnd;
		}
}
