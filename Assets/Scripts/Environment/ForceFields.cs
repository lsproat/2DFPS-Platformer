using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFields : MonoBehaviour
{

    [SerializeField] GameObject tutorialText;
    public GameObject enemies3D;
    public GameObject[] fields;
    Animator animate;

    private bool forceFieldUp = true;

    private void Start()
    {
        animate = tutorialText.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && forceFieldUp)
        {
            tutorialText.SetActive(true);

            //animation
            animate.SetBool("MoveIn", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && forceFieldUp)
        {
            //animation
            animate.SetBool("MoveIn", false);
            animate.SetBool("MoveOut", true);
        }
    }


    private void Update()
    {
        if(enemies3D.transform.childCount == 0)
        {
            forceFieldUp = false;
            foreach (GameObject gates in fields)
            {
                gates.SetActive(false);
            }
        }
    }

}
