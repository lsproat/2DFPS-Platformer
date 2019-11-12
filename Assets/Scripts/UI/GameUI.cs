using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;

    InputManager input;

    public void Start()
    {
        Cursor.visible = false;
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
    }

    public void ShowGameWinUI()
    {
        Time.timeScale = .0000001f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameWinUI.SetActive(true);
    }

    public void ShowGameLoseUI()
    {
        input.inputActive = false;
        Time.timeScale = .0000001f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameLoseUI.SetActive(true);
    }
}
