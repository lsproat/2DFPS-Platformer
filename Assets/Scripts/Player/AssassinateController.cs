using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateController : MonoBehaviour
{
    public GameObject PressToAssassinate;
    private HandleScore scoreScript;
    private GameObject enemy;

    private bool hasCollided = false;
    public KeyCode Melee;
    public int assassiniateScore = 10;

    private void Start()
    {
        scoreScript = gameObject.GetComponent<HandleScore>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject.transform.parent.parent.gameObject;
            hasCollided = true;
            PressToAssassinate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasCollided = false;
        PressToAssassinate.SetActive(false);
    }

    private void Update()
    {
        if (hasCollided)
        {
            if (Input.GetKeyDown(Melee))
            {
                //Destroy(gameObject.transform.parent.parent.gameObject);
                scoreScript.scoreVal += assassiniateScore;
                Destroy(enemy);
                PressToAssassinate.SetActive(false);
                hasCollided = false;
            }
        }
    }
}
    

