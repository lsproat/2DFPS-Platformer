using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    public float jumpHeight = 3.5f;
    public float timeToJumpApex = 0.4f;
    public float jumps = 1;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6f;
    public float graceTime = 0.2f;
    float graceTimer;

    public float jumpCooldown = 0.2f;
    bool isJumping = false;

    float jumpVelocity;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;
    Vector2 input;

    Controller2D controller;
    InputManager inputManager;

    void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        controller = GetComponent<Controller2D>();
    }

    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        graceTimer = graceTime;
    }

    void Update()
    {
        if (controller.collisions.below) graceTimer = graceTime;
        else if (graceTimer <= graceTime) graceTimer -= Time.deltaTime;


        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (inputManager.inputActive)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && !isJumping && (controller.collisions.below || jumps > 0 || graceTimer > 0 )) // TODO: Universal input??
            {
                isJumping = true;
                velocity.y = jumpVelocity;
                if (graceTimer <= 0)
                {
                    jumps--;
                    graceTimer = graceTime;
                }
                Invoke("JumpCoolingDown", jumpCooldown);
            }
            if (controller.collisions.below) jumps = 1;
        }
        else input = new Vector2(0.0f, 0.0f);

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void JumpCoolingDown()
    {
        isJumping = false;
    }
}
