using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public GameObject gameUI;
    GameUI ui;

    private void Start()
    {
        ui = gameUI.GetComponent<GameUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reload") SceneManager.LoadScene(1);
        else if (other.gameObject.tag == "Win")
        {
            Destroy(other.gameObject);
            ui.ShowGameWinUI();
        }
    }

    public void UIButtonRetry()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);
    }

    public void UIButtonReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
