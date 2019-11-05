using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateController : MonoBehaviour
{
    public GameObject PressToAssassinate;
    private bool hasCollided = false;
    public KeyCode Melee;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasCollided = true;
            PressToAssassinate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasCollided = false;
        PressToAssassinate.SetActive(false);
    }

    private void Update()
    {
        if (hasCollided)
        {
            if (Input.GetKeyDown(Melee))
            {
                Destroy(gameObject.transform.parent.parent.gameObject);
                PressToAssassinate.SetActive(false);
                hasCollided = false;
            }
        }
    }
}
    

