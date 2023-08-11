using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public Button BackButton;
    public Button ResumeButton;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetButton("Pause"))
        {
            if(!gameIsPaused)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
    }
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ResumeButton.Select();

    }

    public void SelectedBackButton()
    {
        BackButton.Select();
    }

    public void SelectResumeButton()
    {
        ResumeButton.Select();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
