using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDoubleJumpAnimation : MonoBehaviour
{
    Animator animate;

    private void Start()
    {
        animate = GetComponent<Animator>();
        Debug.Log(gameObject.name);
    }

    public void HandleDoubleJumpAnimation()
    {
        animate.SetBool("DoubleJump", false);
        Debug.Log("okay");
    }


}
