using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject fpPlayer;
    [SerializeField] GameObject player2D;

    private void Awake()
    {
        fpPlayer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CamSwitch")
        {
            fpPlayer.SetActive(true);
            player2D.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
