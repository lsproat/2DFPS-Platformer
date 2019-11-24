using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCrystal : MonoBehaviour
{
    HandleScore score;
    public int scoreGiven = 100;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("yo");
            score = collision.gameObject.GetComponent<HandleScore>();
            score.scoreVal += scoreGiven;
            Destroy(gameObject);
            //TODO: play sound

        }
    }
}
