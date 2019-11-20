﻿using UnityEngine;
using System.Collections;

public class CameraPathFollower : MonoBehaviour
{

    public float speed = 3f;
    public Transform pathParent;
    Transform targetPoint;
    int index;
    public Transform crystal;

    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;
        for (int a = 0; a < pathParent.childCount; a++)
        {
            from = pathParent.GetChild(a).position;
            to = pathParent.GetChild((a + 1) % pathParent.childCount).position;
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(from, to);
        }
    }
    void Start()
    {
        index = 0;
        targetPoint = pathParent.GetChild(index);
        gameObject.transform.position = targetPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(crystal);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            index++;
            index %= pathParent.childCount;
            targetPoint = pathParent.GetChild(index);
        }
    }
}