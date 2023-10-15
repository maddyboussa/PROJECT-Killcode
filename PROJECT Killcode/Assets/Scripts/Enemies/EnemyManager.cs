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
        void Start()
        {
            basicEnemyList = new List<GameObject>();
            SpawnMultEnemies(numStartEnemies);
        }

        // Update is called once per frame
        void Update()
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
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawningArea.center, spawningArea.size);
        }
    }
}
