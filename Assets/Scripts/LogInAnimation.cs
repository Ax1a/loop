using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class LogInAnimation : MonoBehaviour
{
    [SerializeField] private GameObject logInPanel;
    [SerializeField] private float typingSpeed = 0.5f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private GameObject loading;

    private Tween fadeTween;

    private void OnEnable() {
        StartCoroutine(PlayLoginAnimation());
    }

    IEnumerator PlayLoginAnimation() {
        logInPanel.SetActive(true);
        passwordText.text = "";

        for (int i = 0; i < 8; i++)
        {
            passwordText.text += "*";
            yield return new WaitForSeconds(typingSpeed);
        }

        loginText.gameObject.SetActive(false);
        loading.SetActive(true);
        yield return new WaitForSeconds(1f);

        FadeOut();
        yield return new WaitForSeconds(animationDuration);
        logInPanel.SetActive(false);

        // Reset
        canvasGroup.alpha = 1;
        loginText.gameObject.SetActive(true);
        loading.SetActive(false);
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

    private void FadeOut()
    {
        Fade(0f, animationDuration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
}
