using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Beings the game by loading the first level
    /// </summary>
    public void PlayGame()
    {
        // load the level with index 1 from the build queue
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits the application
    /// Note: This will be ignored in the editor
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
