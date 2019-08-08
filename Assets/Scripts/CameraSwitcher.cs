using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject fpPlayer;
    [SerializeField] GameObject player2D;

    bool activeFP = false;
    bool active2D = true;

    private void Awake()
    {
        // initally set FP controlls to OFF
        fpPlayer.SetActive(false);
        gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
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

    private void Toggle2D()
    {
        active2D = !active2D; // used to toggle for next pickup

        if (active2D) gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true; // called every time to ensure logic doesnt mess it up

        player2D.SetActive(active2D);

        gameObject.GetComponent<PlayerMovement2D>().enabled = active2D;
    }

    private void ToggleFP()
    {
        activeFP = !activeFP;

        if (activeFP) gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionX;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        fpPlayer.SetActive(activeFP);
        gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = activeFP;
    }

}
