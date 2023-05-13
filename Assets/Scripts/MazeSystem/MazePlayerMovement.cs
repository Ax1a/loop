using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazePlayerMovement : MonoBehaviour
{
    [Header ("References")]
    public Transform orientation;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsCrouchObstacle;
    bool grounded;

    [Header ("Enums")]
    public MovementState state;

    // Private variables
    float horizontalInput;
    float verticalInput;
    [SerializeField] private Vector3 startingPosition;
    Vector3 moveDirection;
    Rigidbody rb;
    private bool isCrouching;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
        startingPosition = transform.position;
    }

    private void Update()
    {
        if (!ThirdPersonCamera.Instance.IsControlEnabled()) return;
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        if (!ThirdPersonCamera.Instance.IsControlEnabled()) return;
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            isCrouching = true;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            // Check if there's an obstacle above the player
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.17f, whatIsCrouchObstacle);
            if (hitColliders.Length > 0)
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }

        // check if the player is still under an obstacle
        if (isCrouching || Input.GetKey(crouchKey))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.17f, whatIsCrouchObstacle);
            if (hitColliders.Length > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                isCrouching = true;
            }
            else if (hitColliders.Length == 0 && !Input.GetKey(crouchKey)) {
                isCrouching = false;
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }
        else {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
        }
    }

    private void StateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            else {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

        // Mode - Walking
        else if (grounded && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }

        // Mode - Air
        else if (!grounded)
        {
            state = MovementState.air;
        }
        else {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else if(!grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
    }
}