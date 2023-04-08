using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BlockTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipTxt;
    [SerializeField] private float animationDuration;
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween fadeTween;
    private bool isFadeOut = false;

    public void SetTooltipText(string text) {
        tooltipTxt.text = text;
    }

    public void FadeOut()
    {
        isFadeOut = true;

        Fade(0f, animationDuration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });

    }

    public void FadeIn() {
        isFadeOut = false;
        gameObject.SetActive(true);

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

        fadeTween = canvasGroup.DOFade(endValue, duration).OnComplete(() => {
            if (isFadeOut) Destroy(gameObject);
        });
        fadeTween.onComplete += onEnd;
    }
}
