using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameObject playerParent;

    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private float jumpSpeed = 8.0F;
    [SerializeField] private float gravity = 20.0F;
    [SerializeField] private KeyCode jumpKey;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintBuildUpSpeed;
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private KeyCode crouchKey;

    Vector3 moveDirection;

    private InputManager inputManager;
    private CharacterController charController;
    private Camera cam;
    private Vector3 camOrginalPosition;
    private float movementSpeed;
    private bool isJumping;

    private float vertInput;
    private float horizInput;
    private float jumpedVertInput;
    private float jumpedHorizInput;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        charController = GetComponentInParent<CharacterController>();
        cam = playerParent.GetComponentInChildren<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        SetMovementSpeed();

        if (inputManager.inputActive)
        {
            // Because of how the 2D system works, the horizontalInputName and verticalInputName need to be switched
            vertInput = -(Input.GetAxis(horizontalInputName));
            horizInput = Input.GetAxis(verticalInputName);
        }
        else
        {
            vertInput = 0f;
            horizInput = 0f;
            movementSpeed = 0;
            moveDirection.x = 0f;
            moveDirection.z = 0f;
        }
        MoveAndJump();

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    private void MoveAndJump()
    {
        if (charController.isGrounded) // On ground movement
        {
            moveDirection = new Vector3(horizInput, 0f, vertInput);
            moveDirection.Normalize();
            moveDirection = playerParent.transform.TransformDirection(moveDirection);
            moveDirection *= movementSpeed;
            if (Input.GetKeyDown(jumpKey))
            {
                moveDirection.y = jumpSpeed;
                jumpedHorizInput = horizInput;
                jumpedVertInput = vertInput;
            }
            else if (Input.GetKeyDown(crouchKey) && movementSpeed > 0f) //&& vertInput > 0f)
            {
                camOrginalPosition = cam.transform.localPosition;
                // ProcessCrouchAndSlide(); NEEDS WORK
            }
        }
        else if (!charController.isGrounded) // In air movement
        {
            // If player stops input mid-air, continue momentum
            if (Mathf.Abs(Input.GetAxis(horizontalInputName)) < 1 && Mathf.Abs(Input.GetAxis(verticalInputName)) < 1)
            {
               moveDirection.x = (jumpedHorizInput + horizInput) * movementSpeed;
               moveDirection.z = (jumpedVertInput + vertInput) * movementSpeed;

            }
            // enable in air control
            else
            {
                moveDirection.x = horizInput * movementSpeed;
                moveDirection.z = vertInput * movementSpeed;
            }
            moveDirection = playerParent.transform.TransformDirection(moveDirection);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        charController.Move(moveDirection * Time.deltaTime);
       // Debug.Log("vert input: " + vertInput + " horiz input: " + horizInput);
        
    }

    private void ProcessCrouchAndSlide()
    {
        if (Input.GetKeyUp(crouchKey)) cam.transform.localPosition = camOrginalPosition;
        else if (movementSpeed <= walkSpeed)
        {
            // crouch
            cam.transform.localPosition = new Vector3(0f, 0.45f, 0f);
        }
        else if (movementSpeed >= sprintSpeed)
        {
            // slide
            moveDirection.z = 0f;
            cam.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            moveDirection.x = Mathf.SmoothDamp(moveDirection.x, 0f, ref movementSpeed, .1f);
        }
        
    }

    private void SetMovementSpeed()
    {
        if (Input.GetKey(sprintKey))
        {
            movementSpeed = Mathf.Lerp(movementSpeed, sprintSpeed, Time.deltaTime * sprintBuildUpSpeed);
        }
        else movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, Time.deltaTime * sprintBuildUpSpeed);
    }

    private bool OnSlope()
    {
        if (isJumping) return false;

        RaycastHit hit;
        if (Physics.Raycast(playerParent.transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up) return true;
        }
        return false;
    }
}
