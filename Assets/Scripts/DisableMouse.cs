using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouse : MonoBehaviour
{
    private void OnEnable() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
