using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] private float timeBetweenPoints = 1f;
    [SerializeField] List<Vector3> walkPoints;

    float currentLerpTime;
    int count = 0;
    int count2 = 0;

    bool dectectedPlayer = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Start()
    {
        IdleMovement();
    }

    void IdleMovement()
    {
        while (!dectectedPlayer)
        {
            if (count == walkPoints.Count - 1) count2 = 0;
            else count2 = count++;
            //MoveFromAToB(walkPoints[count], walkPoints[count2]);
            Invoke("MoveFromAToB", timeBetweenPoints);

            count++;
        }
    }

    void MoveFromAToB()
    {
        Vector3 startPos = walkPoints[count];
        Vector3 endPos = walkPoints[count2];

        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > timeBetweenPoints)
        {
            currentLerpTime = timeBetweenPoints;
        }

        //lerp!
        float perc = currentLerpTime / timeBetweenPoints;
        transform.position = Vector3.Lerp(startPos, endPos, perc);
        // TODO: Lerp to look at next target
        //transform.rotation = Quaternion.Lerp(startRot, endRot, perc);
    }
}
