using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] float waitTime = 5f;

    PlayerMovement2D controller2D;
    Rigidbody rb;
    UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controllerFP;
    Animator animate;

    // These handle the logic for toggling controllers and camera direction 
    bool activeFP = false;
    bool active2D = true;

    private void Awake()
    {
        //Initiliaze all GetComponet calls
        controller2D = gameObject.GetComponent<PlayerMovement2D>();
        controllerFP = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        rb = gameObject.GetComponent<Rigidbody>();
        animate = gameObject.GetComponentInChildren<Animator>();

        // initally set FP controls to OFF
        controllerFP.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CamSwitch")
        {
            MoveCamera();
            ToggleFP();
            Toggle2D();
            Destroy(other.gameObject); //destory pickip
        }
    }

    private void MoveCamera()
    {
        return;
    }

    private void ToggleFP()
    {
        activeFP = !activeFP; // used to toggle for next pickup

        // Controlls script and rigidbody attributes/enabled status
        if (activeFP)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            animate.SetTrigger("Zoom In");

        }
        rb.freezeRotation = true;  // TODO: Affects FP controls??
        controllerFP.enabled = activeFP;
    }

    private void Toggle2D()
    {
        active2D = !active2D;

        if (active2D)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            animate.SetTrigger("Zoom Out");

        }
        rb.freezeRotation = true; 
        controller2D.enabled = active2D;
    }

    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(waitTime);
    }
}
