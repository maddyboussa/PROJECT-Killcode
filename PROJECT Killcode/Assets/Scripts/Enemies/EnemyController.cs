using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Killcode.Core;
using AYellowpaper.SerializedCollections;
using Killcode.Events;
using Killcode.Player;

namespace Killcode.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onEnemyDeath;
        [SerializeField] private Stats stats;
        [SerializeField] private float damage;
        [SerializeField] private SerializedDictionary<Stat, float> localStats;
        private Rigidbody2D rb;
        [SerializeField] private bool isDead;

        [SerializeField] private GameObject target;
        public GameObject Target { get { return target; } set { target = value; } }
        BoxCollider2D eBoxCollider;

        // get reference to other active enemies
        private List<GameObject> enemyList;

        private Vector3 direction = new Vector3(0, 0, 0);
        Vector3 desiredVelocity;

        [Header("Movement Stats")]
        [SerializeField] private bool lerp;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            eBoxCollider = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            localStats = new SerializedDictionary<Stat, float>(stats.instanceStats);
            isDead = false;

            // make sure this enemy has a reference to all other active enemies
            enemyList = GameObject.Find("EnemyManager").GetComponent<EnemyManager>().BasicEnemyList;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            AddForce(0.5f);
        }

        /// <summary>
        /// Adds force to move enemy
        /// </summary>
        /// <param name="lerpAmount">Amount to smooth movement by</param>
        private void AddForce(float lerpAmount)
        {
            SeekPlayer();

            // Attempt to get maxSpeed stat and calculate velocity
            Vector2 targetSpeed = direction * stats.GetStat(Stat.maxSpeed);

            // If using lerp, then apply lerp
            if (lerp)
            {
                // Reduce our control using Lerp() this smooths changes to are direction and speed
                targetSpeed.x = Mathf.Lerp(rb.velocity.x, targetSpeed.x, lerpAmount);
                targetSpeed.y = Mathf.Lerp(rb.velocity.y, targetSpeed.y, lerpAmount);
            }

            // Calculate difference between current velocity and desired velocity
            Vector2 speedDif = targetSpeed - rb.velocity;

            // Attempt to get accelrate and calculate acceleration and force of movement
            Vector2 movement = speedDif * stats.GetStat(Stat.accelRate);

            // Convert this to a vector and apply to rigidbody
            rb.AddForce(movement, ForceMode2D.Force);
        }

        /// <summary>
        /// Update direction to face towards target (player)
        /// </summary>
        private void SeekPlayer()
        {
            // get vector from enemy to target position
            Vector3 steeringDirection = target.transform.position - this.transform.position;

            // set direction to vector towards player
            direction = steeringDirection.normalized;
        }

        /// <summary>
        /// Reduce enemy health by a certain amount
        /// </summary>
        /// <param name="damageAmount">amount to damage enemy by</param>
        public void TakeDamage(float damageAmount)
        {
            Debug.Log(damageAmount);

            // Try to get the health of the enemy
            if (localStats.TryGetValue(Stat.hitPoints, out float currentHealth))
            {
                // reduce enemy health
                localStats[Stat.hitPoints] = currentHealth - damageAmount;

                Debug.Log("damage" + localStats[Stat.hitPoints]);

                // check for enemy death
                if (localStats[Stat.hitPoints] <= 0.0f)
                {
                    isDead = true;
                    onEnemyDeath.Raise(this, gameObject);
                }
            }
        }

        /// <summary>
        /// Checks for collision between this enemy's box collider and a player's box collider
        /// </summary>
        /// <param name="collision"></param>
        public void OnCollisionEnter2D(Collision2D collision)
        {
            // check if colliding with a player
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                // if colliding, reduce player health according to this enemy's damage
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                
                return;
            }
        }
    }
}
