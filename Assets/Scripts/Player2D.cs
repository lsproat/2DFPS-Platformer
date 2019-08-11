using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    public float jumpHeight = 3.5f;
    public float timeToJumpApex = 0.4f;
    float accelerationTimeAirborne = 0.2f;
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6f;

    float jumpVelocity;
    float gravity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    
    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector3 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below) // TODO: Universal input??
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
