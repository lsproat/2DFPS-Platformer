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

    InputManager inputManager;
    PerspectiveSwitcher lerpCamView;
    ZoomInLerp lerpIN;
    ZoomOutLerp lerpOUT;

    // These handle the logic for toggling controllers and camera direction 
    bool activeFP = false;
    bool active2D = true;


    private void Awake()
    {
        //Initiliaze all GetComponet calls
        inputManager = GetComponentInParent<InputManager>();
        lerpCamView = gameObject.GetComponentInChildren<PerspectiveSwitcher>();
        lerpIN = cameraMain.GetComponent<ZoomInLerp>();
        lerpOUT = cameraMain.GetComponent<ZoomOutLerp>();

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
            inputManager.inputActive = false; // stop player input
            ChangePerspective();
            Destroy(other.gameObject); //destory pickip
        }
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
        lerpIN.enabled = true;                              // start camera lerp
        lerpCamView.ChangeProjection = true;                // start camera perspective lerp 
        yield return new WaitForSeconds(coroutineWaitTime); // (CAN CUT OFF LERP PROGRESS) wait before enabling control
        controller2D.SetActive(false);
        lerpIN.enabled = false;
        inputManager.inputActive = true;
        ToggleControlFP();                                  // enable FP controls

    }

    IEnumerator LerpingOUT()
    {
        gameObject.transform.rotation = Quaternion.identity; // reset parent rotation
        lerpOUT.enabled = true;
        lerpCamView.ChangeProjection = true;
        yield return new WaitForSeconds(coroutineWaitTime);
        controllerFP.SetActive(false);
        lerpOUT.enabled = false;
        inputManager.inputActive = true;
        ToggleControl2D();

    }

    private void ToggleControlFP()
    {
        controllerFP.SetActive(true);
        //gameObject.transform.rotation = Quaternion.Euler(0f, 360f, 0f); // TODO: (doesnt work) Keep direction same when switching to FP
    }

    private void ToggleControl2D()
    {
        controller2D.SetActive(true);

        cameraMain.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Ensure camera is facing direction of 2D gameplay
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0); // ensure 0 Z pos when FP -> 2D
    }
}
