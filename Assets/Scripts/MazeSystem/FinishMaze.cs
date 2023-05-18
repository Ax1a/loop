using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishMaze : MonoBehaviour
{
    [SerializeField] MazeManager mazeManager;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject forceField;
    private bool canInteract = false, triggered = false;
    Coroutine coroutine;

    private void Start() {
        triggered = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (mazeManager != null)
            {
                mazeManager.CheckForWin();
                MazeUI.Instance.SetInteractionIndicator("Open Gate");
                canInteract = true;

            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        canInteract = false;
        MazeUI.Instance.DisableInteractionIndicator();
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            if (mazeManager.canExit && !triggered)
            {
                triggered = true;
                MazeUI.Instance.DisableInteractionIndicator();
                MazeUI.Instance.ShowSuccessPopup("Blocks Completed!");
                if (coroutine == null)
                    coroutine = StartCoroutine(OpenGate());
            }
            else
            {
                //Play Sfx 
                AudioManager.Instance.PlaySfx("Loose");
                MazeUI.Instance.ShowAlertPopup("Collect all the blocks first!");
            }
        }
    }

    private IEnumerator OpenGate() {
        // Add sound effect
        button.transform.DOLocalMoveY(0.548f, .5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.5f);
        button.transform.DOLocalMoveY(0.593f, .5f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.PlaySfx("Win");
        forceField.SetActive(false);
        coroutine = null;
    }
}
