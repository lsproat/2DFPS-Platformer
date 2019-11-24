using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateController : MonoBehaviour
{
    public GameObject PressToAssassinate;
    public GameObject meleeSoundGO;
    private HandleScore scoreScript;
    private GameObject enemy;
    AudioSource meleeSound;

    public GameObject model3D;

    Animator animate;
    private bool hasCollided = false;
    public KeyCode Melee;
    public int assassiniateScore = 10;
    public bool assassinateAnimationDone = true;

    private void Start()
    {
        scoreScript = gameObject.GetComponent<HandleScore>();
        animate = model3D.GetComponent<Animator>();
        meleeSound = meleeSoundGO.GetComponent<AudioSource>();
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
        if (Input.GetKeyDown(Melee) && assassinateAnimationDone && model3D.activeSelf)
        {
            animate.SetTrigger("attack");
            meleeSound.PlayDelayed(0.2f);
            assassinateAnimationDone = false;
        }
    }

    public void AssassinateTarget()
    {
        if (hasCollided)
        {
            scoreScript.scoreVal += assassiniateScore;
            Destroy(enemy);
            PressToAssassinate.SetActive(false);
            hasCollided = false;
        }
    }
}
    

