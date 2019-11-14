using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    Vector3 levelStartPlayerPos;

    public GameObject gameUI;
    GameUI ui;
    CameraSwitcher pos;
    CharacterController charController;


    private void Start()
    {
        ui = gameUI.GetComponent<GameUI>();
        pos = GetComponent<CameraSwitcher>();

        charController = GetComponent<CharacterController>();
        levelStartPlayerPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reload")
        {
            RestartFromCheckpoint();
        }
        else if (other.gameObject.tag == "Win") ui.ShowGameWinUI();
    }

    public void RestartFromCheckpoint()
    {
        if (pos.checkpointPos != Vector3.zero) //there is a checkpoint
        {
            //this is a workaround for an issue stemming from the character controller for 3D
            charController.enabled = false;
            charController.transform.position = pos.checkpointPos;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0); //reset rotation for checkpoint
            charController.enabled = true;
        }
        else // checkpoint has not been set yet
        {
            gameObject.transform.position = levelStartPlayerPos;
        }
    }
}
