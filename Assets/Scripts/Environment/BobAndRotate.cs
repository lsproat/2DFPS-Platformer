using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAndRotate : MonoBehaviour
{
    public float bobbingHeight;
    public int speed;
    BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Update()
    {
        //Rotating
        transform.RotateAround(collider.bounds.center, new Vector3(0, 1, 0), speed);
    }
}
