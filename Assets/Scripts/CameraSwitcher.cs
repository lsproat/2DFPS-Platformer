using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject cameraMain;
    [SerializeField] float coroutineWaitTime = 5;

    PlayerMovement2D controller2D;
    Rigidbody rb;
    UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController controllerFP;

    ZoomInLerp lerpIN;
    ZoomOutLerp lerpOUT;

    // These handle the logic for toggling controllers and camera direction 
    bool activeFP = false;
    bool active2D = true;

    CameraProjectionChange lerpCamView;

    private void Awake()
    {
        //Initiliaze all GetComponet calls
        controller2D = gameObject.GetComponent<PlayerMovement2D>();
        controllerFP = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        rb = gameObject.GetComponent<Rigidbody>();
        lerpIN = cameraMain.GetComponent<ZoomInLerp>();
        lerpOUT = cameraMain.GetComponent<ZoomOutLerp>();
        lerpCamView = gameObject.GetComponentInChildren<CameraProjectionChange>();

        // initally set FP controls to OFF
        controllerFP.enabled = false;

        //disable Lerps
        lerpIN.enabled = false;
        lerpOUT.enabled = false;
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

        if (activeFP) StartCoroutine(LerpingIN());
    }


    private void Toggle2D()
    {
        active2D = !active2D;

        if (active2D) StartCoroutine(LerpingOUT());
    }

    IEnumerator LerpingOUT()
    {
        controllerFP.enabled = false; //Stop player control
        gameObject.transform.rotation = Quaternion.identity; // reset parent rotation

        lerpOUT.enabled = true;
        lerpCamView.ChangeProjection = true;
        yield return new WaitForSeconds(coroutineWaitTime);
        lerpOUT.enabled = false;
        ToggleControl2D();

    }

    IEnumerator LerpingIN()
    {
        controller2D.enabled = false; //Stop player control

        lerpIN.enabled = true;
        lerpCamView.ChangeProjection = true;
        yield return new WaitForSeconds(coroutineWaitTime);

        lerpIN.enabled = false;
        ToggleControlFP();

    }

    private void ToggleControlFP()
    {
        controllerFP.enabled = true;
        gameObject.transform.rotation = Quaternion.Euler(0f, 360f, 0f); // TODO: (doesnt work) Keep direction same when switching to FP

            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
    }

    private void ToggleControl2D()
    {
        controller2D.enabled = true;
        rb.drag = 0.0f; //FP cntroller alters this value

        cameraMain.transform.rotation = Quaternion.Euler(0f, 270f, 0f);

        gameObject.transform.position = new Vector3(0, transform.position.y, transform.position.z); // ensure 0 X pos when FP -> 2D

        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.freezeRotation = true;
    }
}
