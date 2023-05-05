using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    public CharacterController controller;
    private bool _isPanelActive = false;
    private bool disableControl = false;
    private float turnSmoothVelocity, horizontal, vertical;
    private GameObject mainUI;
    private GameObject _currentCamera;
    private Animator _animator;
    private Vector3 direction;
     
    private void Start() {
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!_isPanelActive && !disableControl && !BotGuide.Instance.guideIsActive()){
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
         
            _currentCamera = CameraManager.Instance.GetCurrentCamera();
            
            direction = (_currentCamera.transform.forward * vertical + _currentCamera.transform.right * horizontal).normalized;

            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                controller.Move(direction * speed * Time.deltaTime);
                _animator.SetBool("IsMoving", true);
            }
            else {
                _animator.SetBool("IsMoving", false);
            }
        }
        else {
            _animator.SetBool("IsMoving", false);
        }
    }

    public void SetIsPanelActive(bool active) {
        _isPanelActive = active;
    }

    public void ToggleControl(bool isDisable) {
        disableControl = isDisable;
    }

    public bool IsPanelActive() {
        return _isPanelActive;
    }
}
