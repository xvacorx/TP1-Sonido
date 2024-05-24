using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject tutorialUI;

    public bool gameIsPaused = true;
    public bool gameStarted = false;
    public bool inOptions = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !FindObjectOfType<GameManager>().gameOver && gameStarted)
        {
            if (gameIsPaused && !inOptions)
            {
                Resume();
                BackSound();
            }
            else if (!gameIsPaused && !inOptions)
                Pause();
            else if (gameIsPaused && inOptions)
            {
                pauseMenuUI.SetActive(true);
                optionsMenu.SetActive(false);
                BackSound();
                Options();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        FindObjectOfType<AudioManager>().Play("Pause");
        Cursor.lockState = CursorLockMode.None;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void MainMenu()
    {
        ConfirmSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        if (!inOptions)
        {
            inOptions = true;
        }
        else
        {
            inOptions = false;
        }
    }

    public void QuitGame()
    {
        ConfirmSound();
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        Cursor.lockState= CursorLockMode.Locked;
        Time.timeScale = 1f;
        tutorialUI.SetActive(false);
        gameIsPaused = false;
        gameStarted = true;
    }

    public void ConfirmSound()
    {
        FindObjectOfType<AudioManager>().Play("Confirm");
    }

    public void BackSound()
    {
        FindObjectOfType<AudioManager>().Play("Back");
    }
}
