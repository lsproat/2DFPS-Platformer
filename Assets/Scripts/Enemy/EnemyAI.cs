using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    [SerializeField] Transform playerTarget;

    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float turnDelay = 3;
    [SerializeField] int speed = 5;
    [SerializeField] float startWaitTime = 6;
    public Vector3[] points;
    public bool playerDetected = false;
    private float waitTime;


    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    private int destPoint = 0;
    private bool turnEnemy = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false; //continuous motion between points (no slowdown)
        navMeshAgent.updateRotation = false;

        destPoint++; // assume enemy starts in destPoint[0];
        waitTime = startWaitTime;
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        //if counter reaches end, set 0 to loop to begining spot
        if (destPoint == points.Length) destPoint = 0;

        transform.position = Vector3.MoveTowards(transform.position, points[destPoint], speed * Time.deltaTime);

        // when enemy reaches destination
        if (Vector3.Distance(transform.position, points[destPoint]) < 0.2f)
        {
            if (waitTime <= 0)
            {
                waitTime = startWaitTime;
                destPoint++;
            }
            else if (waitTime <= turnDelay)
            {
                int tmpDestPoint = destPoint;
                if (tmpDestPoint + 1 == points.Length) tmpDestPoint = 0;
                else tmpDestPoint++;

                FaceTarget(points[tmpDestPoint]);
                waitTime -= Time.deltaTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    private void EngageTarget()
    {
        FaceTarget(playerTarget.position);
        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            //capture/attack
        }
    }

    private void ChaseTarget()
    {
        navMeshAgent.SetDestination(playerTarget.position);
    }

    private void FaceTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        // shows detected radius (not being used)
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}