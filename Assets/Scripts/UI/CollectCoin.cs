using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] GameObject player;
    private HandleScore scoreScript;
    float coinVolume = 10;
    [SerializeField] GameObject coinSoundGO;
    AudioSource coinSound;

    private void Start()
    {
        scoreScript = player.GetComponent<HandleScore>();
        coinSound = coinSoundGO.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            coinSound.Play();
            scoreScript.scoreVal++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            coinSound.Play();
            scoreScript.scoreVal++;
            Destroy(gameObject);
        }
    }
}
