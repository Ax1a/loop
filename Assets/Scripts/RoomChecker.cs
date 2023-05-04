using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] string roomName;
    [SerializeField] Light[] lights;
    [SerializeField] private GameObject character;
    [SerializeField] private Clock clock;
    private int lightIntensity;
    private PlayerController _playerController;

    private void Start() {
        CameraManager.Instance.SetCurrentCamera("Camera");
        _playerController = character.GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other) {
        if (clock.Hour > 18 || (clock.Hour >= 0 && clock.Hour <= 6)) {
            lightIntensity = 30;
        }
        else {
            lightIntensity = 10;
        }

        if(other.gameObject.name != "Player" || _playerController.IsPanelActive()){
            return;
        }
        // Kitchen Camera
        if (roomName == "kitchen") {
            CameraManager.Instance.SetCurrentCamera("KitchenCamera");
            lights[1].intensity = 15;
            lights[0].intensity = 0;
        }
        // Main Room Camera
        else if (roomName == "main") {
            CameraManager.Instance.SetCurrentCamera("Camera");
            lights[1].intensity = 0;
            lights[0].intensity = lightIntensity;
        }
    }
}
