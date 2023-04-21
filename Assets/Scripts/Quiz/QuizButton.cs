using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class QuizButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("Hover");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("OptionSelect");
    }

}