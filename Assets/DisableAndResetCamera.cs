using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAndResetCamera : MonoBehaviour
{
    [SerializeField] Camera otherCam;
    public void AnimateEvent()
    {
        otherCam.enabled = true;
        gameObject.GetComponent<Camera>().enabled = false;
        gameObject.transform.position = new Vector3(0, 0, 0);
    }
}
