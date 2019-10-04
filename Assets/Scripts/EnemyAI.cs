using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Transform playerTarget;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float waitBeforeMove = 3f;
    public Vector3[] points;
    public bool playerDetected = false;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    private int destPoint = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false; //continuous motion between points (no slowdown)
        navMeshAgent.updateRotation = false;

        GotoNextPoint();
    }

    void Update()
    {
        // distanceToTarget = Vector3.Distance(target.position, transform.position);
        if (!playerDetected && !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
            //GotoNextPoint();
            WaitAndRotate();
       //else if (playerDetected) EngageTarget();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        navMeshAgent.destination = points[destPoint];

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
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

    IEnumerator WaitAndRotate()
    {
        //int tmpDestPoint = (destPoint + 1) % points.Length;
        Vector3 tmpDestination = points[destPoint];
        FaceTarget(tmpDestination);
        yield return new WaitForSeconds(waitBeforeMove);
        GotoNextPoint();

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