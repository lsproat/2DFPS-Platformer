using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;

    GameObject player;
    InputManager input;
    Checkpoints checkpoint;

    public void Start()
    {
        Cursor.visible = false;
        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<InputManager>();
        checkpoint = player.GetComponent<Checkpoints>();


        //performance
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 30;
    }

    public void ShowGameWinUI()
    {
        //Time.timeScale = .0000001f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameWinUI.SetActive(true);
        input.inputActive = false;
    }

    public void ShowGameLoseUI()
    {
        input.inputActive = false;
        //Time.timeScale = .0000001f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameLoseUI.SetActive(true);
    }

    public void UIButtonRetry()
    {
        Debug.Log("fuck me bitch ass hoe");
        //Time.timeScale = 1f;
        gameLoseUI.SetActive(false);
        input.inputActive = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        checkpoint.RestartFromCheckpoint();
    }

    public void UIButtonReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
