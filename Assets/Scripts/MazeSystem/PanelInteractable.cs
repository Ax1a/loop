using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInteractable : MonoBehaviour
{
    void Start()
    {
        if (ThirdPersonCamera.Instance != null)
        {
            ThirdPersonCamera.Instance.ToggleControl(false);
            ThirdPersonCamera.Instance.ToggleCursor(true);
        }
    }
    private void OnEnable()
    {
        if (ThirdPersonCamera.Instance != null)
        {
            ThirdPersonCamera.Instance.ToggleControl(false);
            ThirdPersonCamera.Instance.ToggleCursor(true);
        }
    }

    private void OnDisable()
    {
        if (ThirdPersonCamera.Instance != null)
        {
            ThirdPersonCamera.Instance.ToggleControl(true);
            ThirdPersonCamera.Instance.ToggleCursor(false);
        }
    }
}
