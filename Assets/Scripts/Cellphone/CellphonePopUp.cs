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
    private bool _applicationOpen;
    Sequence sequence;

    void Start ()
    {
        _playerController = Character.GetComponent<PlayerController>();
        cellphone.gameObject.SetActive(false);
    }
    void LateUpdate ()
    {
        if (_playerController.IsPanelActive()) {
            if (!_isActive || _applicationOpen) return;
            if (sequence != null) {
                sequence.Kill();
            }
            sequence = DOTween.Sequence();
            sequence.Append(cellphone.DOAnchorPos(new Vector2(0, -606), animationDuration).SetEase(ease).OnComplete(() => cellphone.gameObject.SetActive(false)));

            return;
        }
        if (Input.GetKeyDown(InputManager.Instance.openPhone) && !UIController.Instance.otherPanelActive())
        {
            if (sequence != null) {
                sequence.Kill();
            }
            cellphone.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(cellphone.DOAnchorPos(Vector2.zero, animationDuration).SetEase(ease));
            _isActive = true;
        }
        else if (Input.GetKeyDown(InputManager.Instance.closePhone))
        {
            if (sequence != null) {
                sequence.Kill();
            }
            sequence = DOTween.Sequence();
            sequence.Append(cellphone.DOAnchorPos(new Vector2(0, -606), animationDuration).SetEase(ease).OnComplete(() => cellphone.gameObject.SetActive(false)));
            _isActive = true;
        }
    }

    public void OpenApplication (GameObject applicationPanel)
    {
        if (!applicationPanel.activeSelf)
        {
            if (DataManager.GetTutorialProgress() == 2) {
                UIController.Instance.DequeuePopupHighlight(0);
                BotGuide.Instance.AddDialogue("From here, you can browse through the available quests and messages to see what interests you or needs your attention.");
                BotGuide.Instance.ShowDialogue();
            }
            _applicationOpen = true;
            applicationPanel.SetActive(true);
            UIController.Instance.SetPanelActive(true);
        }
        else 
        {
            UIController.Instance.SetPanelActive(false);
            applicationPanel.SetActive(false);
            _applicationOpen = false;
        }
    }

    public bool IsPhoneActive() {
        return _isActive;
    }
}
