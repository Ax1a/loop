using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] private float speed = 6f;
    private float turnSmoothVelocity, horizontal, vertical;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private Vector3 direction;
    [SerializeField] private Vector3 startingPosition;

    void Start ()
    {
        startingPosition = transform.position;
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * speed * Time.deltaTime);
        }

    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
    }
}
