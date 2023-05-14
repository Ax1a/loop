using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeComputer : MonoBehaviour
{
    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private GameObject computerCamera;
    private bool canInteract;
    Coroutine animateOpenComputer;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (computerCanvas != null) {
                canInteract = true;
                MazeUI.Instance.SetInteractionIndicator("Open System");
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        canInteract = false;
        MazeUI.Instance.DisableInteractionIndicator();
    }

    private void LateUpdate() {
        if (canInteract && Input.GetKeyDown(KeyCode.E) && !computerCanvas.activeInHierarchy) {
            if (animateOpenComputer == null)
                animateOpenComputer = StartCoroutine(OpenComputer());
        }
    }

    private IEnumerator OpenComputer() {
        computerCamera.SetActive(true);
        yield return new WaitForSeconds(1f);
        computerCanvas.SetActive(true);
        animateOpenComputer = null;
    }

    public void TurnOffComputer() {
        computerCanvas.SetActive(false);
        computerCamera.SetActive(false);
    }
}
