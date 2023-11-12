using Killcode.Enemy;
using Killcode.Events;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Killcode.Weapons
{
    public class SniperBullet : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletActive;
        [SerializeField] private Rect currentBounds;
        [SerializeField] private float pierceTimer;
        private float damage;
        private bool dealDamage;
        #endregion

        /// <summary>
        /// Activate the bullet
        /// </summary>
        public void ActivateBullet()
        {
            onBulletActive.Raise(this, currentBounds);
        }

        /// <summary>
        /// Set the bullet fields
        /// </summary>
        /// <param name="weaponInfo"></param>
        public void SetBullet(SniperWeaponInfo weaponInfo)
        {
            dealDamage = true;

            // Set starting position
            transform.position = weaponInfo.startingPoint;

            // Stretch the square sprite
            Vector3 scale = transform.localScale;
            scale.x = weaponInfo.distance;
            transform.localScale = scale;

            // Rotate the square sprite in 2D space
            transform.rotation = Quaternion.Euler(0f, 0f, weaponInfo.angle);

            // Reset color
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 1f;
            GetComponent<SpriteRenderer>().color = color;

            // Start fading the bullet
            StartCoroutine(FadeBullet());
        }

        /// <summary>
        /// On death, set the gameobject to inactive
        /// </summary>
        private void OnDeath()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Update the bounds of the bullet
        /// </summary>
        /// <param name="sender">The component raising the event</param>
        /// <param name="data">The data being sent</param>
        public void UpdateBounds(Component sender, object data)
        {
            if (data is Rect)
            {
                currentBounds = (Rect)data;
            }
        }

        /// <summary>
        /// Set the bullet damage
        /// </summary>
        /// <param name="newDamage">The damage of the bullet</param>
        public void SetBulletDamage(float newDamage)
        {
            damage = newDamage;
        }

        /// <summary>
        /// Fade the bullet
        /// </summary>
        /// <returns></returns>
        protected IEnumerator FadeBullet()
        {
            // Initially wait for a second
            yield return new WaitForSeconds(0.25f);

            // Start countdown
            float waitTime = pierceTimer;
            float counter = 0f;

            while (counter < waitTime)
            {
                // Update the counter
                counter += Time.deltaTime;
                
                // Stop dealing damage after a certain amount of time
                if((counter / waitTime) >= 0.3f)
                {
                    dealDamage = false;
                }

                // Retrieve the color
                Color color = gameObject.GetComponent<SpriteRenderer>().color;

                // Subtract from the opacity
                color.a -= (counter / waitTime)/300f;

                // Set the coor
                gameObject.GetComponent<SpriteRenderer>().color = color;

                //Don't freeze Unity
                yield return null;
            }

            // Destroy the sniper bullet
            OnDeath();
        }

        /// <summary>
        /// Checks for collision between this bullet's box collider and other colliders
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<EnemyController>() && dealDamage)
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
                return;
            }
        }
    }
}
