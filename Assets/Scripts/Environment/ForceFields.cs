using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFields : MonoBehaviour
{

    [SerializeField] GameObject tutorialText;
    public GameObject enemies3D;
    public GameObject[] fields;
    Animator animate;

    AudioSource fieldDown;
    private bool forceFieldUp = true;

    private int enemyCount;
    [HideInInspector] public int enemiesKilled = 0;

    private void Start()
    {
        animate = tutorialText.GetComponent<Animator>();
        fieldDown = GetComponent<AudioSource>();

        enemyCount = enemies3D.transform.childCount;
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
        if (enemyCount == enemiesKilled && forceFieldUp)
        {
            forceFieldUp = false;
            fieldDown.Play();
            foreach (GameObject gates in fields)
            {
                gates.SetActive(false);
            }
        }
    }

}
