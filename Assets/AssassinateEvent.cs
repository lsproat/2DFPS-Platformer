using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateEvent : MonoBehaviour
{
    AssassinateController boop;

    // Start is called before the first frame update
    void Start()
    {
        boop = GetComponentInParent<AssassinateController>();
    }

    public void OnMeleePress()
    {
        boop.AssassinateTarget();
    }
}
