using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChecker : MonoBehaviour
{
    [SerializeField] string roomName;
    [SerializeField] GameObject _camera;
    [SerializeField] Light[] lights;
    bool changedRoom = false;

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.name != "Player"){
            return;
        }
       
        // Kitchen Camera
        if (roomName == "kitchen") {
            _camera.transform.position = new Vector3(4.63000011f, 9.67000008f, -8.51000023f);
            changedRoom = true;
            lights[1].intensity = 8;
        }
        // Main Room Camera
        else if (roomName == "main") {
            _camera.transform.position = new Vector3(-1.98000002f, 9.27999973f, -11.0799999f);
            lights[1].intensity = 0;
        }
    }
}
