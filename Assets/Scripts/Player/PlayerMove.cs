using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameObject playerParent;

    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintBuildUpSpeed;
    [SerializeField] private KeyCode sprintKey;

    private InputManager inputManager;
    private CharacterController charController;
    private float movementSpeed;
    private bool isJumping;

    private float vertInput;
    private float horizInput;

    private void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        charController = GetComponentInParent<CharacterController>();
        
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
        }


        Vector3 forwardMovement = playerParent.transform.forward * vertInput;
        Vector3 rightMovement = playerParent.transform.right * horizInput;

        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1f) * movementSpeed);

        if((vertInput != 0 || horizInput != 0) && OnSlope())
        {
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);
        }

        JumpInput();
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

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90f;

        float timeInAir = 0;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;

            yield return null;
        }
        while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45f;

        isJumping = false;
    }
}
