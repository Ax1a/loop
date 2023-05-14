using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    [Header ("Params")]
    [SerializeField] private float rotationspeed;
    private bool isControlEnabled = true;
    public static ThirdPersonCamera Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    void Start ()
    {
        ToggleCursor(false);
    }

    void Update()
    {
        if (!isControlEnabled) {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
        }
        else {
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
        }
        if (!isControlEnabled) return;

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player object
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationspeed);
    }

    public void ToggleCursor(bool enable) {
        if (enable) {
            // enable cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            // Disable cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ToggleControl(bool enable) {
        isControlEnabled = enable;
    }

    public bool IsControlEnabled() {
        return isControlEnabled;
    }
}
