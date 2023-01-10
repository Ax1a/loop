using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public GameObject _camera;
    [SerializeField] private float speed = 6f;

    [SerializeField] private float turnSmoothTime = 0.1f;
    private bool panelActive = false;
    float turnSmoothVelocity;
    [SerializeField] private GameObject[] panels;
    private GameObject mainUI;

    void Update()
    {

        foreach (GameObject panel in panels)
        {
            if (panel.activeInHierarchy == true) {
                panelActive = true;
                return;
            }
            else {
                panelActive = false;
            }
        }

        if (panelActive == false) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 forward = _camera.transform.forward;

            Vector3 direction = forward * vertical + _camera.transform.right * horizontal;
            
            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                controller.Move(direction * speed * Time.deltaTime);
            }
        }

        //add money press c        
        #if UNITY_EDITOR
        if(Input.GetKey(KeyCode.C))
            DataManager.AddMoney(50);
        #endif

        GameSharedUI.Instance.UpdateMoneyUITxt();
    }
}
