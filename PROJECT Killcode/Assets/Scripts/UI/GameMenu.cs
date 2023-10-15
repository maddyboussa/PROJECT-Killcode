using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;
using Killcode.Core;

public class GameMenu : MonoBehaviour
{
    #region FIELDS
    // get reference to pause ui
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathMenu;
    private bool paused;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // make sure pause and death ui are inactive on start
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        paused = false;
    }

    public void Pause()
    {
        // first check that player has not died
        if (!deathMenu.activeSelf)
        {
            // check if menu is already paused
            if (paused)
            {
                // if so, unpause game
                Resume();
            }
            // if unpaused, pause game and activate pause menu
            else
            {
                // activate pause menu canvas
                pauseMenu.SetActive(true);

                // freeze time
                Time.timeScale = 0.0f;

                paused = true;
            }
        }
    }

    /// <summary>
    /// Resumes game, hiding pause menu
    /// </summary>
    public void Resume()
    {
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);

        // unfreeze time
        Time.timeScale = 1.0f;

        paused = false;
    }

    /// <summary>
    /// Restarts game
    /// </summary>
    public void Restart()
    {
        // load first level again
        SceneManager.LoadScene(1);

        // unfreeze time
        Time.timeScale = 1.0f;

        paused = false;
    }

    /// <summary>
    /// Loads main menu
    /// </summary>
    public void MainMenu()
    {
        // load the main menu (has index 0)
        SceneManager.LoadScene(0);

        // unfreeze time
        Time.timeScale = 1.0f;
    }

    public void ShowDeathMenu()
    {
        deathMenu.SetActive(true);

        // unfreeze time
        Time.timeScale = 0.0f;

    }
}
