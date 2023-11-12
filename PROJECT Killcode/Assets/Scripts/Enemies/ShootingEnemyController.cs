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
    public class ShootingEnemyController : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onEnemyDeath;
        [SerializeField] private Stats stats;
        [SerializeField] private SerializedDictionary<Stat, float> localStats;
        [SerializeField] private GameObject target;
        private Rigidbody2D rb;

        [Header("Stats")]
        [SerializeField] private float damage;
        public bool isDead;
        public bool active = false;

        [Header("Movement")]
        [SerializeField] private bool lerp;
        public Vector3 position = new Vector3(0, 0, 0);
        private Vector3 direction = new Vector3(0, 0, 0);
        #endregion

        #region PROPERTIES
        public GameObject Target { get { return target; } set { target = value; } }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            transform.position = position;
            rb = GetComponent<Rigidbody2D>();
            localStats = new SerializedDictionary<Stat, float>(stats.instanceStats);
            isDead = false;
            active = false;
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
            // If the enemy is not active, return
            if (!active)
            {
                return;
            }

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
            // Try to get the health of the enemy
            if (localStats.TryGetValue(Stat.hitPoints, out float currentHealth) && active)
            {
                // reduce enemy health
                localStats[Stat.hitPoints] = currentHealth - damageAmount;

                // check for enemy death
                if (localStats[Stat.hitPoints] <= 0.0f && !isDead)
                {
                    isDead = true;
                    onEnemyDeath.Raise(this, gameObject);
                }
            }
        }

        /// <summary>
        /// Activate the enemy
        /// </summary>
        /// <param name="sender">The component raising the event</param>
        /// <param name="data">The data being sent</param>
        public void ActivateEnemy(Component sender, object data)
        {
            active = true;
        }

        /// <summary>
        /// Checks for collision between this enemy's box collider and a player's box collider
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerEnter2D(Collider2D collision)
        {
            // check if colliding with a player
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                // If active, damage the player
                if (active)
                {
                    // if colliding, reduce player health according to this enemy's damage
                    collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                }

                return;
            }
        }
    }
}