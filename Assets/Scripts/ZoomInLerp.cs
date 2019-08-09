using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInLerp : MonoBehaviour
{
    [SerializeField] private Transform endObject;


    [SerializeField] private float lerpTime = 1f;
    float currentLerpTime;
    float moveDistance;

    Vector3 startPos;
    Vector3 endPos;

    Quaternion startRot;
    Quaternion endRot;

    protected void Start()
    {
        float moveDistance = Vector3.Distance(transform.position, endObject.position);
        startPos = transform.position;
        startRot = transform.rotation;
    }

    protected void Update()
    {
        endRot = endObject.rotation;
        endPos = endObject.transform.position;

        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        //lerp!
        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);
        transform.rotation = Quaternion.Lerp(startRot, endRot, perc);
    }
}
