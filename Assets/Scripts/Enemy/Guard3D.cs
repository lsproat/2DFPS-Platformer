﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard3D : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float waitTime = 0.3f;
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float timeToSpotPlayer = 0.5f;

    public Transform pathHolder;
    public LayerMask viewMask;
    private Animator animate;

    public VLight spotlight;
    [SerializeField] float viewDistance;

    Color orignalSpotlightColor;
    Transform player;
    public GameObject canvas;

    public float addedHeadHeight;

    float viewAngle;
    float playerVisibleTimer;

    private void Start()
    {
        Time.timeScale = 1; // bug fix for the win/lose pause screen

        animate = gameObject.GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        orignalSpotlightColor = spotlight.colorTint;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];

        for (int i = 0; i< waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    private void Update()
    {
        if (CanSeePlayer()) playerVisibleTimer += Time.deltaTime;
        else playerVisibleTimer -= Time.deltaTime;

        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.colorTint = Color.Lerp(orignalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            canvas.SendMessage("ShowGameLoseUI");
        }
    }

    bool CanSeePlayer()
    {
        if(Vector3.Distance(new Vector3(transform.position.x, transform.position.y + addedHeadHeight, transform.position.z), player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if(angleBetweenGuardAndPlayer < viewAngle/2)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true; //can see player
                }
            }
        }
        return false; //cant see player
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        if (waypoints.Length > 1)
        {
            transform.position = waypoints[0];

            int targetWaypointIndex = 1;
            Vector3 targetWaypoint = waypoints[targetWaypointIndex];
            transform.LookAt(targetWaypoint);

            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
                //animate Trigger - Walking
                animate.SetBool("IsTurning", false);
                animate.SetBool("IsWalking", true);

                if (transform.position == targetWaypoint)
                {
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                    targetWaypoint = waypoints[targetWaypointIndex];

                    //animate Trigger - Idle
                    animate.SetBool("IsTurning", false);
                    animate.SetBool("IsWalking", false);


                    yield return new WaitForSeconds(waitTime);

                    //animate Trigger - Turning
                    animate.SetBool("IsTurning", true);
                    animate.SetBool("IsWalking", false);

                    yield return StartCoroutine(TurnToFace(targetWaypoint));
                }
                yield return null;
            }
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(directionToLookTarget.z, directionToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + addedHeadHeight, transform.position.z), transform.forward * viewDistance);
    }
}
