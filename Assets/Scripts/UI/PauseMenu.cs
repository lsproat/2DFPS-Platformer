using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pause;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsMenuUI;

    private bool isPaused = false;

    InputManager input;

    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
    }

    private void Update()
    {
        // get input from puase key
        if(Input.GetKeyDown(pause))
        {
            if (!isPaused)
            {
                isPaused = true;
                pauseMenuUI.SetActive(true);
                input.inputActive = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = .0000001f;
            }
            else if (isPaused) ResumeGameButton();

        }
    }

    public void BackToPauseScreenButton()
    {
        pauseMenuUI.SetActive(true);
        settingsMenuUI.SetActive(false);
    }

    public void ResumeGameButton()
    {
        isPaused = false;
        input.inputActive = true;
        Time.timeScale = 1;
        Cursor.visible = false;

        if (pauseMenuUI.activeSelf) pauseMenuUI.SetActive(false);
        else settingsMenuUI.SetActive(false);
    }

    public void SettingsButton()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadCurrentLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
