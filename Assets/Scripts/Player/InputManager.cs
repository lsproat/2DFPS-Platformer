using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public bool inputActive;

    private void Awake()
    {
        inputActive = true;
    }
}
