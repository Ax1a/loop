using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CellphonePopUp : MonoBehaviour
{
    [SerializeField] private RectTransform cellphone;
    [SerializeField] private float animationDuration;
    [SerializeField] private Ease ease;
    [SerializeField] private GameObject Character;
    PlayerController _playerController;
    private bool _isActive;

    void Start ()
    {
        _playerController = Character.GetComponent<PlayerController>();

    }
    void Update ()
    {
        if (_playerController.IsPanelActive()) return;
        if (Input.GetKeyDown(InputManager.Instance.openPhone) && !UIController.Instance.otherPanelActive())
        {
            cellphone.DOAnchorPos(Vector2.zero, animationDuration).SetEase(ease);
        }
        else if (Input.GetKeyUp(InputManager.Instance.closePhone))
        {
            cellphone.DOAnchorPos(new Vector2(0, -606), animationDuration).SetEase(ease);
        }
    }

    public void OpenApplication (GameObject applicationPanel)
    {
        if (!applicationPanel.activeSelf)
        {
            _isActive = true;
            applicationPanel.SetActive(true);
            UIController.Instance.SetPanelActive(true);
        }
        else 
        {
            UIController.Instance.SetPanelActive(false);
            _isActive = false;
            applicationPanel.SetActive(false);
        }
    }

    public bool IsPhoneActive() {
        return _isActive;
    }
}
