using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent (typeof(Button))]
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{

    TextMeshProUGUI txt;
    Color baseColor;
    Button btn;
    bool interactableDelay;
    bool isSelected;
    [SerializeField] Color accentColor;
    [SerializeField] bool forNavigation = false;

    void Start ()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        baseColor = txt.color;
        btn = gameObject.GetComponent<Button> ();
        interactableDelay = btn.interactable;
    }

    void Update ()
    {
        if (btn.interactable != interactableDelay) {
            if (btn.interactable) {
                txt.color = baseColor;
            } else {
                txt.color = baseColor * btn.colors.disabledColor * btn.colors.colorMultiplier;
            }
        }
        interactableDelay = btn.interactable;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        if (btn.interactable) {
            txt.color = accentColor;
        } else {
            txt.color = baseColor * btn.colors.disabledColor * btn.colors.colorMultiplier;
        }

        AudioManager.Instance.PlaySfx("Hover");
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (btn.interactable) {
            txt.color = accentColor;
        } else {
            txt.color = baseColor * btn.colors.disabledColor * btn.colors.colorMultiplier;
        }
       AudioManager.Instance.PlaySfx("Button");
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (btn.interactable) {
            if (forNavigation) {
                txt.color = baseColor;
            }
            else {
                txt.color = accentColor;
            }
        } else {
            txt.color = baseColor * btn.colors.disabledColor * btn.colors.colorMultiplier;
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (isSelected == true && forNavigation == false) return;

        if (btn.interactable) {
            txt.color = baseColor;
        } else {
            txt.color = baseColor * btn.colors.disabledColor * btn.colors.colorMultiplier;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        txt.color = accentColor;
        isSelected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        txt.color = baseColor;
        isSelected = false;
    }

}