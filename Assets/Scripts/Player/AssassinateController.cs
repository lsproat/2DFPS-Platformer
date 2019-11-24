using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinateController : MonoBehaviour
{
    public GameObject PressToAssassinate;
    public GameObject meleeSoundGO;
    public GameObject deathSoundGO;
    public GameObject ForceFieldCrystal;
    private HandleScore scoreScript;
    private GameObject enemy;
    AudioSource meleeSound;
    AudioSource deathSound;

    public GameObject model3D;

    ForceFields counter;
    Animator animate;
    Animator enemyAnimate;
    private bool hasCollided = false;
    public KeyCode Melee;
    public int assassiniateScore = 10;

    [HideInInspector]
    public bool assassinateAnimationDone = true;

    private void Awake()
    {
        scoreScript = gameObject.GetComponent<HandleScore>();
        animate = model3D.GetComponent<Animator>();
        meleeSound = meleeSoundGO.GetComponent<AudioSource>();
        deathSound = deathSoundGO.GetComponent<AudioSource>();
        counter = ForceFieldCrystal.GetComponent<ForceFields>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemy = other.gameObject;
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
            enemyAnimate = enemy.transform.parent.GetComponentInChildren<Animator>();
            Guard3D enemyGuardScript = enemy.transform.parent.GetComponent<Guard3D>();
            Transform enemySpotlight = enemy.transform.parent.GetChild(1);
            Transform enemyAssassinationCollider = enemy.transform.parent.GetChild(2);
            enemy = enemy.transform.parent.parent.gameObject;

            //add score
            scoreScript.scoreVal += assassiniateScore;

            //add to counter for crystal
            counter.enemiesKilled++;

            // disable light/guard script and play death animation/sound
            enemySpotlight.gameObject.SetActive(false);
            enemyGuardScript.StopAllCoroutines();
            enemyGuardScript.enabled = false;
            enemyAssassinationCollider.gameObject.SetActive(false);

            enemyAnimate.SetTrigger("assassinated");
            deathSound.Play();


            Destroy(enemy, 5f);
            PressToAssassinate.SetActive(false);
            hasCollided = false;
        }
    }
}
    

