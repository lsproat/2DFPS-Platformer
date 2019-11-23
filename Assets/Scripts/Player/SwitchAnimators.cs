using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SwitchAnimators : MonoBehaviour
{
    Animator animator;
    RuntimeAnimatorController newController;

    void Start()
    {
        animator = GetComponent<Animator>();
    }



    public void controllerSwap(GameObject FPController, GameObject Controller2D)
    {
        if (Controller2D.activeSelf)
        {
            newController = (RuntimeAnimatorController)Resources.Load("Cyborg 2D");
        }
        else if (FPController.activeSelf)
        {
            newController = (RuntimeAnimatorController)Resources.Load("Cyborg 3D");
        }

        animator.runtimeAnimatorController = newController;
    }

}
