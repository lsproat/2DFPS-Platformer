using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject cameraMain;
    [SerializeField] float coroutineWaitTime = 5;

    [SerializeField] GameObject controllerFP;
    [SerializeField] GameObject controller2D;

    Rigidbody rb;
    CameraProjectionChange lerpCamView;

    ZoomInLerp lerpIN;
    ZoomOutLerp lerpOUT;

    // These handle the logic for toggling controllers and camera direction 
    bool activeFP = false;
    bool active2D = true;


    private void Awake()
    {
        //Initiliaze all GetComponet calls
        rb = gameObject.GetComponentInChildren<Rigidbody>();
        lerpIN = cameraMain.GetComponent<ZoomInLerp>();
        lerpOUT = cameraMain.GetComponent<ZoomOutLerp>();
        lerpCamView = gameObject.GetComponentInChildren<CameraProjectionChange>();

        // initally set FP controls to OFF
        controllerFP.SetActive(false);

        //disable Lerps
        lerpIN.enabled = false;
        lerpOUT.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CamSwitch")
        {
            AdjustPlayerPosition(); // removes velocity so player does not fall off level
            ChangePerspective();
            Destroy(other.gameObject); //destory pickip
        }
    }


    private void AdjustPlayerPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.constraints = ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY);
    }

    private void ChangePerspective()
    {
        // used to toggle for next pickup
        activeFP = !activeFP; 
        active2D = !active2D;

        if (activeFP) StartCoroutine(LerpingIN());
        else if (active2D) StartCoroutine(LerpingOUT());

    }


    IEnumerator LerpingIN()
    {
        controller2D.SetActive(false);                      //Stop player control

        lerpIN.enabled = true;                              // start camera lerp
        lerpCamView.ChangeProjection = true;                // start camera perspective lerp 
        yield return new WaitForSeconds(coroutineWaitTime); // (CAN CUT OFF LERP PROGRESS) wait before enabling control

        lerpIN.enabled = false;                             
        ToggleControlFP();                                  // enable FP controls

    }

    IEnumerator LerpingOUT()
    {
        controllerFP.SetActive(false);
        gameObject.transform.rotation = Quaternion.identity; // reset parent rotation

        lerpOUT.enabled = true;
        lerpCamView.ChangeProjection = true;
        yield return new WaitForSeconds(coroutineWaitTime);
        lerpOUT.enabled = false;
        ToggleControl2D();

    }

    private void ToggleControlFP()
    {
        controllerFP.SetActive(true);
        gameObject.transform.rotation = Quaternion.Euler(0f, 360f, 0f); // TODO: (doesnt work) Keep direction same when switching to FP
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
    }

    private void ToggleControl2D()
    {
        controller2D.SetActive(true);
        rb.drag = 0.0f; //FP controller alters this value

        cameraMain.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        gameObject.transform.position = new Vector3(0, transform.position.y, transform.position.z); // ensure 0 X pos when FP -> 2D
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.freezeRotation = true; // TODO: Will this mess with player model FP animations?
    }
}
