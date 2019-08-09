using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInLerp : MonoBehaviour
{
    [SerializeField] private Transform endPos;

    [SerializeField] private float speed = 2f;

    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(transform.position, endPos.position, speed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, endPos.rotation, speed * Time.deltaTime);
    }
}
