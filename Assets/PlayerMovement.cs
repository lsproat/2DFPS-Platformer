using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

     Rigidbody body;
    Vector3 velocity;

    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float jumpForce;

    float horizontal;
    bool isGrounded;

    void Start()
    {
        // Obtain the reference to our Rigidbody.
        body = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                body.AddForce(transform.up * jumpForce);
            }
        }
        if (Input.GetButton("Sprint"))
        {
            velocity = (transform.forward * horizontal) * sprintSpeed * Time.fixedDeltaTime;
        }
        else
        {
            velocity = (transform.forward * horizontal) * speed * Time.fixedDeltaTime;
        }

        velocity.y = body.velocity.y;
        body.velocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            isGrounded = false;
        }
    }
}