using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject settings;
    public GameObject mainMenuIcons;

    public void playButtonUI()
    {
        SceneManager.LoadScene(1);
    }

    public void quitButtonUI()
    {
        Application.Quit();
    }

    public void settingsButton()
    {
        settings.SetActive(true);
        mainMenuIcons.SetActive(false);
    }

    public void backButton()
    {
        settings.SetActive(false);
        mainMenuIcons.SetActive(true);
    }
}
