using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject _camera;
    private bool _isPanelActive = false;
    float turnSmoothVelocity;
    private GameObject mainUI;
    private Animator _animator;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private GameObject[] panels;

    private void Start() {
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Debug.Log(_isPanelActive);
        if (_isPanelActive == false){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 forward = _camera.transform.forward;

            Vector3 direction = (forward * vertical + _camera.transform.right * horizontal).normalized;
            
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

        //add money press c        
        #if UNITY_EDITOR
        if(Input.GetKey(KeyCode.C))
            DataManager.AddMoney(50);
        #endif

        GameSharedUI.Instance.UpdateMoneyUITxt();
    }

    public void SetIsPanelActive(bool active) {
        _isPanelActive = active;
    }
}
