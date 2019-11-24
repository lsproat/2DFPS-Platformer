using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCrystal : MonoBehaviour
{
    HandleScore score;
    public int scoreGiven = 100;
    AudioSource collectSound;
    public GameObject collectSoundGO;

    private void Start()
    {
        collectSound = collectSoundGO.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            score = collision.gameObject.GetComponent<HandleScore>();
            score.scoreVal += scoreGiven;
            Destroy(gameObject);
            collectSound.Play();

        }
    }
}
