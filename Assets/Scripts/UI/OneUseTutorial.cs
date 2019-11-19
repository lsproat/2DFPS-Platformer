using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUseTutorial : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
