using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Killcode.Events;
using System;

namespace Killcode.Levels
{
    public class WaveManager : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onSpawnEnemies;
        [SerializeField] private GameEvent onActivateEnemies;
        [SerializeField] private GameEvent onEndLevel;
        [SerializeField] private LevelData currentLevelData;
        [SerializeField] private int currentWave = 0;
        [SerializeField] private float waveCooldown;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            StartWave();
        }

        /// <summary>
        /// Begin a wave
        /// </summary>
        public void StartWave()
        {
            // Increment current wave
            currentWave++;
            Debug.Log($"Wave {currentWave} of {currentLevelData.wavePositions.Count}");

            // Spawn enemies
            onSpawnEnemies.Raise(this, currentLevelData.wavePositions[currentWave - 1]);

            // Start the countdown
            StartCoroutine(WaveCountdown());
        }

        /// <summary>
        /// End a wave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public void OnWaveEnd(Component sender, object data)
        {
            // Check if the last wave just ended
            if (currentWave == currentLevelData.wavePositions.Count)
            {
                Debug.Log("Changing level");

                // End the level
                onEndLevel.Raise(this, this);
            }
            else
            {
                // Start the next wave
                StartWave();
            }
        }

        /// <summary>
        /// Start a countdown before the wave
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaveCountdown()
        {
            // Wait for the cooldown to end
            yield return new WaitForSeconds(waveCooldown);

            // Activate enemies
            onActivateEnemies.Raise(this, this);
        }
    }
}
