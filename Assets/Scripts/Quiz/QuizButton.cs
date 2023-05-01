using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class QuizButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.DOScale(1.03f, .3f).SetEase(Ease.InOutExpo);
        AudioManager.Instance.PlaySfx("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.DOScale(1f, .3f).SetEase(Ease.InOutExpo);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.DOScale(.97f, .3f).SetEase(Ease.InOutExpo).OnComplete(() => gameObject.transform.DOScale(1f, .3f).SetEase(Ease.InOutExpo));
        AudioManager.Instance.PlaySfx("OptionSelect");
    }

}