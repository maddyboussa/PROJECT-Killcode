using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Killcode.Core;
using AYellowpaper.SerializedCollections;
using Killcode.Events;

namespace Killcode.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region FIELDS
        private Rigidbody2D rb;
        [SerializeField] private GameEvent onPlayerDeath;
        [SerializeField] private GameEvent updateHealthBar;
        [SerializeField] private Stats stats;
        [SerializeField] private SerializedDictionary<Stat, float> localStats;
        private Vector3 direction = new Vector3(0, 0, 0);

        private bool isFacingRight;
        private bool isDead;

        [Header("Movement Stats")]
        [SerializeField] private bool lerp;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            localStats = new SerializedDictionary<Stat, float>(stats.instanceStats);
            isDead = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            // Call run
            Run(1f);
        }

        /// <summary>
        /// Allow the Player to Run
        /// </summary>
        /// <param name="lerpAmount">The amount to smooth movement by</param>
        private void Run(float lerpAmount)
        {
            // Attempt to get maxSpeed stat and calculate velocity
            Vector2 targetSpeed = direction * stats.GetStat(Stat.maxSpeed);

            // If using lerp, then apply lerp
            if(lerp)
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
        /// Turn the Player to face the direction they are moving in
        /// </summary>
        private void Turn()
        {
            // Stores scale and flips the player along the x axis, 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            isFacingRight = !isFacingRight;
        }

        /// <summary>
        /// Get movement input
        /// </summary>
        /// <param name="context">The Callback Context from the Move event</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            // Get the direction vector based on player input
            direction = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Reduce player health by a certain amount
        /// </summary>
        /// <param name="damageAmount">amount to damage enemy by</param>
        public void TakeDamage(float damageAmount)
        {
            // Try to get the health of the player
            if (localStats.TryGetValue(Stat.hitPoints, out float currentHealth))
            {
                // reduce player health
                localStats[Stat.hitPoints] = currentHealth - damageAmount;

                // check for player death
                if (localStats[Stat.hitPoints] <= 0.0f)
                {
                    isDead = true;
                    onPlayerDeath.Raise(this, gameObject);
                }
            }

            // raise event to update game ui (health bar)
            updateHealthBar.Raise(this, localStats[Stat.hitPoints]);
        }
    }
}
