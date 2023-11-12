using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Killcode.Events;
using UnityEngine.Rendering;

namespace Killcode.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onWaveEnd;
        [SerializeField] private GameObject basicEnemyPrefab;
        [SerializeField] private int numStartEnemies;
        [SerializeField] private GameObject enemyTarget;

        // stores active basic enemies
        [SerializeField] private List<GameObject> basicEnemyList;
        public List<GameObject> BasicEnemyList { get { return basicEnemyList; } }

        // stores area enemies can be spawned in
        [SerializeField] private Rect spawningArea;

        #endregion
        // Start is called before the first frame update
        void Awake()
        {
            basicEnemyList = new List<GameObject>();
        }

        private void Update()
        {

        }

        /// <summary>
        /// Spawns a desired number of enemies at specific locations
        /// </summary>
        private void SpawnMultEnemies(int numEnemies)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                // create game object at a random position
                // for right now, spawn enemies within camera bounds - can def change this later
                GameObject newEnemy = Instantiate(basicEnemyPrefab, GetRandPositionInBounds(), Quaternion.identity);

                // set the enemy's target to the player
                newEnemy.GetComponent<EnemyController>().Target = enemyTarget;

                // add to list of active enemies
                basicEnemyList.Add(newEnemy);
            }
        }

        /// <summary>
        /// spawns a single enemy
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="data">Ther enemy sending the event</param>
        public void SpawnSingleEnemy(Component sender, object data)
        {
            // we can change the implementation of this method later
            // for now, respawn a basic enemy to keep game going
            SpawnMultEnemies(1);
        }

        /// <summary>
        /// Returns a random vector within the current bounds (camera)
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRandPositionInBounds()
        {
            // generate random vector within the bounds
            return new Vector3(Random.Range(spawningArea.xMin, spawningArea.xMax), Random.Range(spawningArea.yMin, spawningArea.yMax), 0);
        }


        /// <summary>
        /// Remove an enemy from the Basic Enemy List
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="data">The enemy to remove</param>
        public void RemoveEnemy(Component sender, object data)
        {
            // Check if the data is the correct data type
            if(data is GameObject)
            {
                // Check if the basic enemy list contains the enemy
                if(basicEnemyList.Contains((GameObject)data))
                {
                    // Remove the enemy from the list
                    basicEnemyList.Remove((GameObject)data);

                    // Destroy the object
                    Destroy((GameObject)data);

                    // If there are no more enemies, start the next wave
                    if(basicEnemyList.Count == 0)
                    {
                        onWaveEnd.Raise(this, this);
                    }
                }
            }
        }

        /// <summary>
        /// Spawn enemies according to a List of data
        /// </summary>
        /// <param name="sender">The component raising the event</param>
        /// <param name="data">The data being sent</param>
        public void OnSpawnEnemies(Component sender, object data)
        {
            // If the data is not the current type, return
            if(!(data is List<Vector3>))
            {
                return;
            }

            // Cast the data
            List<Vector3> enemyPositions = (List<Vector3>)data;

            // Loop through the list of enemy positions
            foreach(Vector3 pos in enemyPositions)
            {
                // Spawn an enemy at the position
                GameObject newEnemy = Instantiate(basicEnemyPrefab, pos, Quaternion.identity);
                newEnemy.GetComponent<EnemyController>().position = pos;

                // Set the enemy target to the current target - typically the player
                newEnemy.GetComponent<EnemyController>().Target = enemyTarget;

                // Add the enemy to the list of enemies
                basicEnemyList.Add(newEnemy);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawningArea.center, spawningArea.size);
        }
    }
}
