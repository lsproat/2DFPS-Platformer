using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnXYAxis : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
       transform.position = new Vector3(target.transform.position.x, target.transform.position.y+5, transform.position.z);
    }
}
