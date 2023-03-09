using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    private Camera _mainCam;
    void Start()
    {
        _mainCam = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt( _mainCam.transform);
    }
}
