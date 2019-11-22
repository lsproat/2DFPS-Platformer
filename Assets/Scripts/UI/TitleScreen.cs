using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject settings;
    public GameObject mainMenuIcons;
    public GameObject levelSelect;

    AudioSource buttonSound;

    private void Awake()
    {
        buttonSound = GetComponent<AudioSource>();
    }

    public void playButtonUI()
    {
        buttonSound.Play();

        mainMenuIcons.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void quitButtonUI()
    {
        buttonSound.Play();

        Application.Quit();
    }

    public void settingsButton()
    {
        buttonSound.Play();
        settings.SetActive(true);
        mainMenuIcons.SetActive(false);

    }

    public void backButton()
    {
        if (settings.activeSelf)
        {
            settings.SetActive(false);
            mainMenuIcons.SetActive(true);
        }
        else if (levelSelect.activeSelf)
        {
            levelSelect.SetActive(false);
            mainMenuIcons.SetActive(true);
        }
        buttonSound.Play();
    }

    public void LoadLevelOne()
    {
        buttonSound.Play();
        SceneManager.LoadScene(1);
    }
}
