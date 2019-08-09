using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject cameraFP;
    [SerializeField] GameObject camera2D;

    PlayerMovement2D controller2D;
    Rigidbody rb;
    UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controllerFP;
    Animator animator2D;
    Animator animatorFP;

    // These handle the logic for toggling controllers and camera direction 
    bool activeFP;
    bool active2D;

    private void Awake()
    {
        //Initiliaze all GetComponet calls
        controller2D = gameObject.GetComponent<PlayerMovement2D>();
        controllerFP = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        rb = gameObject.GetComponent<Rigidbody>();
        animator2D = camera2D.GetComponent<Animator>();
        animatorFP = cameraFP.GetComponent<Animator>();

        // initally set FP controls to OFF
        controllerFP.enabled = false;
        cameraFP.SetActive(false);

        activeFP = false;
        active2D = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CamSwitch")
        {
            ToggleFP();
            Toggle2D();
            Destroy(other.gameObject); //destory pickip
        }
    }

    private void ToggleFP()
    {
        activeFP = !activeFP; // used to toggle for next pickup

        if (activeFP) ToggleControlFP();
        //ToggleCameraFP gets called from animator event
    }


    private void Toggle2D()
    {
        active2D = !active2D;

        if (active2D) ToggleControl2D();
        // ToggleCamera2D gets called from animator event
    }

    private void ToggleControlFP()
    {
            controller2D.enabled = false;
            cameraFP.transform.rotation = Quaternion.identity;

            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            animator2D.SetTrigger("Zoom In");
            
    }

    private void ToggleControl2D()
    {
            rb.drag = 0.0f; //FP cntroller alters this value
            controllerFP.enabled = false;

            gameObject.transform.rotation = Quaternion.identity;

            gameObject.transform.position = new Vector3(0, transform.position.y, transform.position.z); // ensure 0 X pos when FP -> 2D

            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.freezeRotation = true;

            animatorFP.SetTrigger("Zoom Out");

            
    }


    //This all happens after animation plays
    public void ToggleCameras()
    {
        if (activeFP)
        {
            Debug.Log("ToggleCams prints me when activeFP = TRUE");
            cameraFP.transform.localPosition = new Vector3(0, 0, 0);
            cameraFP.SetActive(true);
            camera2D.SetActive(false);
            controllerFP.enabled = true;
            animator2D.SetTrigger("Set Zoom In Idle");
        }
        else if(active2D)
        {
            Debug.Log("ToggleCams prints me when active2D = TRUE");
            cameraFP.transform.localPosition = new Vector3(0, 0, 0);
            camera2D.SetActive(true);
            cameraFP.SetActive(false);
            controller2D.enabled = true;
            animatorFP.SetTrigger("Set Zoom Out Idle");
        }
    }
}
