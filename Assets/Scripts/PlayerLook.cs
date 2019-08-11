using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] private Transform playerBody;
    [SerializeField] private string mouseXInputName, mouseYInputName;


    private float xAxisClamp;

    public float mouseSensitivity;

    private void Awake()
    {
        LockCursor(); // TODO: Affects 2D?
        xAxisClamp = 0;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraRoation();
    }

    private void CameraRoation()
    {
        float mouseX = Input.GetAxisRaw(mouseXInputName) * mouseSensitivity;
        float mouseY = Input.GetAxisRaw(mouseYInputName) * mouseSensitivity;

        xAxisClamp += mouseY;

        if(xAxisClamp > 90f)
        {
            xAxisClamp = 90f;
            mouseY = 0f;
            ClampXAxisRoationToValue(270f); ;
        }
        else if (xAxisClamp < -90f)
        {
            xAxisClamp = -90f;
            mouseY = 0f;
            ClampXAxisRoationToValue(90f);
        }

        cam.transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRoationToValue(float value)
    {
        Vector3 eulerRotation = cam.transform.eulerAngles;
        eulerRotation.x = value;
        cam.transform.eulerAngles = eulerRotation;
    }
}
