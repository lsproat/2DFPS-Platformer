using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateEvent : MonoBehaviour
{
    AssassinateController boop;
    Animator animate;

    // Start is called before the first frame update
    void Start()
    {
        boop = GetComponentInParent<AssassinateController>();
        animate = GetComponent<Animator>();
    }

    public void OnMeleePress()
    {
        boop.AssassinateTarget();
        boop.assassinateAnimationDone = true;
    }
}
