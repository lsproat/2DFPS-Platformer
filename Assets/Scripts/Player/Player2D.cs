using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    public float jumpHeight = 3.5f;
    public float timeToJumpApex = 0.4f;
    public int jumps = 1;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6f;
    public float graceTime = 0.2f;
    float graceTimer;

    public float jumpCooldown = 0.2f;
    bool jumpCoolDownCheck = false;

    bool moving = false;
    bool jumpActivated = false;
    float jumpVelocity;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;
    Vector2 input;

    Controller2D controller;
    InputManager inputManager;

    [SerializeField] GameObject playerModel;
    Animator animate;

    [SerializeField] GameObject playerJumpSoundGO;
    AudioSource jumpSound;

    void Awake()
    {
        inputManager = GetComponentInParent<InputManager>();
        jumpSound = playerJumpSoundGO.GetComponent<AudioSource>();
        controller = GetComponent<Controller2D>();
        animate = playerModel.GetComponent<Animator>();
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

        bool grounded = controller.collisions.below;

        if (inputManager.inputActive)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && !jumpCoolDownCheck && (grounded || jumps > 0 || graceTimer > 0 )) // TODO: Universal input??
            {
                jumpCoolDownCheck = true;
                jumpActivated = true;
                velocity.y = jumpVelocity;
                //sound for jump
                jumpSound.Play();

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

        ProcessMovingAnimations(input, grounded);

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        jumpActivated = false;
    }

    private void ProcessMovingAnimations(Vector2 input, bool grounded)
    {
        if (input.x < 0 || input.x > 0) moving = true;
        else moving = false;

        if (jumps > 0) animate.SetBool("DoubleJumpUsed", false);

        // to the left
        if (input.x < 0) playerModel.transform.rotation = Quaternion.Euler(0, 270, 0);
        // to the right 
        else if (input.x > 0) playerModel.transform.rotation = Quaternion.Euler(0, 90, 0);

        if (!grounded && jumpActivated)
        {
            animate.SetBool("DoubleJump", true); //double jump
            if (jumps == 0) animate.SetBool("DoubleJumpUsed", true);
        }
        if (!moving) animate.SetBool("Moving", false); //idle

        if (!grounded && !jumpActivated) animate.SetBool("inAir", true); //fell down
        if (grounded && jumpActivated) animate.SetBool("Jump", true); //first jump
        else if (grounded)
        {
            if (moving) animate.SetBool("Moving", true);

            animate.SetBool("inAir", false);
            animate.SetBool("DoubleJump", false);
            animate.SetBool("Jump", false);
        }
    }

    private void JumpCoolingDown()
    {
        jumpCoolDownCheck = false;
    }
}
