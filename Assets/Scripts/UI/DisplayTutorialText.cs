using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTutorialText : MonoBehaviour
{

    [SerializeField] GameObject tutorialText;
    Animator animate;

    private void Start()
    {
        animate = tutorialText.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tutorialText.SetActive(true);

            //animation
            animate.SetBool("MoveIn", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //animation
            animate.SetBool("MoveIn", false);
            animate.SetBool("MoveOut", true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tutorialText.SetActive(true);

            //animation
            animate.SetBool("MoveIn", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //animation
            animate.SetBool("MoveIn", false);
            animate.SetBool("MoveOut", true);
        }
    }
}
