using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Killcode.Levels
{
    public class LevelManager : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private string nextLevelName;
        #endregion

        /// <summary>
        /// Change the level
        /// </summary>
        /// <param name="sender">The component raising the event</param>
        /// <param name="data">The data being sent</param>
        public void ChangeLevel(Component sender, object data)
        {
            // Change the scene
            SceneManager.LoadScene(nextLevelName);
        }
    }

}