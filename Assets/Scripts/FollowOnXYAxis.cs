using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOnXYAxis : MonoBehaviour
{
    [SerializeField] Transform target;
    public int higherOnY = 3;

    void Update()
    {
       transform.position = new Vector3(target.transform.position.x, target.transform.position.y+higherOnY, target.transform.position.z - 30);
    }
}
