using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public KeyCode pause;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject settingsMenuUI;

    AudioSource buttonSound;

    private bool isPaused = false;

    InputManager input;

    private void Start()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        buttonSound = GetComponentInParent<AudioSource>();
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

        buttonSound.Play();
    }

    public void ResumeGameButton()
    {
        isPaused = false;
        input.inputActive = true;
        Time.timeScale = 1;
        Cursor.visible = false;

        if (pauseMenuUI.activeSelf) pauseMenuUI.SetActive(false);
        else settingsMenuUI.SetActive(false);

        buttonSound.Play();
    }

    public void SettingsButton()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);

        buttonSound.Play();
    }

    public void MainMenuButton()
    {
        buttonSound.Play();

        SceneManager.LoadScene(0);
    }

    public void ReloadCurrentLevel()
    {
        buttonSound.Play();
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
