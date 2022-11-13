using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField] private float speed = 6f;

    [SerializeField] private float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public GameObject panelCom;
    
    void Update()
    {
        if (panelCom.activeInHierarchy == false) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                controller.Move(direction * speed * Time.deltaTime);
            }
        }
    }
}
