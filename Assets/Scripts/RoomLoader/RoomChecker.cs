using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] string roomName;
    [SerializeField] Light[] lights;
    private bool _isPlayerInRoom;

    private void Start() {
        CameraManager.Instance.SetCurrentCamera("Camera");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") {
            _isPlayerInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.name == "Player") {
            _isPlayerInRoom = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name != "Player"){
            return;
        }
        // Kitchen Camera
        if (roomName == "kitchen") {
            CameraManager.Instance.SetCurrentCamera("KitchenCamera");
            lights[1].intensity = 8;
        }
        // Main Room Camera
        else if (roomName == "main") {
            CameraManager.Instance.SetCurrentCamera("Camera");
            lights[1].intensity = 0;
        }
    }
}
