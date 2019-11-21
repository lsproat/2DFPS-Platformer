using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] GameObject player;
    private HandleScore scoreScript;

    private void Start()
    {
        scoreScript = player.GetComponent<HandleScore>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            scoreScript.scoreVal++;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            scoreScript.scoreVal++;
            Destroy(gameObject);
        }
    }
}
