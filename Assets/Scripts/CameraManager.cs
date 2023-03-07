using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private GameObject[] cameras;
    [SerializeField] private GameObject[] transitionCameras;
    private GameObject _currentCamera;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public GameObject GetCurrentCamera() {
        return _currentCamera;
    }

    private IEnumerator DelayCurrentCamera(GameObject camera) {
        yield return new WaitForSeconds(.5f);
        _currentCamera = camera;
    }

    public void SetCurrentCamera(string cameraName) {
        if(_currentCamera != null && _currentCamera.name == cameraName) return;

        foreach (var camera in cameras)
        {
            if (camera.name == cameraName) {
                camera.SetActive(true);
                if (_currentCamera != null) {
                    StartCoroutine(DelayCurrentCamera(camera));
                } else {
                    _currentCamera = camera;
                }
            } else {
                camera.SetActive(false);
            }
        }
    }

    public void ToggleTransitionCamera(string cameraName) {
        if(_currentCamera.name == cameraName) return;

        foreach (var camera in transitionCameras)
        {
            camera.SetActive(false);

           if (camera.name == cameraName) {
                bool isActive = camera.activeSelf;

                camera.SetActive(!isActive);
            }
        }
    }
}
