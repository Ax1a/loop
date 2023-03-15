using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LogInAnimation : MonoBehaviour
{
    [Header("Instances")]
    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject desktopScreen;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private TextMeshProUGUI loginText;
    [SerializeField] private Image loginBtn;
    [SerializeField] private GameObject loading;

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.5f;
    [SerializeField] private float animationDuration = 0.5f;
    public static LogInAnimation Instance;

    private Tween fadeTween;
    private void OnEnable() {
        StartCoroutine(PlayLoginAnimation());
    }

    IEnumerator PlayLoginAnimation() {
        desktopScreen.SetActive(false);
        logInPanel.SetActive(true);
        passwordText.text = "";

        for (int i = 0; i < 8; i++)
        {
            passwordText.text += "*";
            yield return new WaitForSeconds(typingSpeed);
        }

        loginText.gameObject.SetActive(false);

        Color loginPressedColor;
        if (ColorUtility.TryParseHtmlString("#E1E2E3", out loginPressedColor))
        {
            loginBtn.color = loginPressedColor;
        }
        loading.SetActive(true);
        yield return new WaitForSeconds(1f);

        desktopScreen.SetActive(true);
        FadeOut();
        yield return new WaitForSeconds(animationDuration);
        logInPanel.SetActive(false);

        // Reset
        canvasGroup.alpha = 1;
        loginBtn.color = Color.white;
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
