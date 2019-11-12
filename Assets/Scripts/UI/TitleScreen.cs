using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void playButtonUI()
    {
        SceneManager.LoadScene(1);
    }

    public void quitButtonUI()
    {
        Application.Quit();
    }
}
