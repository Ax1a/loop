using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IIndicatorController : MonoBehaviour
{
    public string interactableLayer;
    public GameObject[] indicators;
    private bool _addedIndicators = false;
    
    void Start()
    {
        if (DataManager.GetTutorialProgress() >= 3) {
            foreach (GameObject indicator in indicators)
            {
                indicator.SetActive(true);
            }
            _addedIndicators = true;
        }
        else {
            foreach (GameObject indicator in indicators)
            {
                indicator.SetActive(false);
            }
        }
    }

    private void LateUpdate() {
        // Check if the indicator is already added, no other panel is active, and the tutorial progress is > 2
        if (!_addedIndicators && DataManager.GetTutorialProgress() >= 3 && !UIController.Instance.otherPanelActive() && UIController.Instance.QueueIsEmpty()) {

            foreach (GameObject indicator in indicators)
            {
                indicator.SetActive(true);
            }
            _addedIndicators = true;
        }
    }
}
