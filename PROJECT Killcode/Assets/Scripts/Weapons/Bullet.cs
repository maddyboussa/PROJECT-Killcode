using Killcode.Enemy;
using Killcode.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Killcode.Weapons
{
    public struct BulletSpawnInfo
    {
        public Vector2 bulletPos;
        public Vector2 bulletDirection;
        public float bulletDamage;
        public WeaponType weaponType;
        public object extraWeaponInfo;

        public BulletSpawnInfo(Vector2 bulletPos, Vector2 bulletDirection, float bulletDamage, WeaponType weaponType)
        {
            this.bulletPos = bulletPos;
            this.bulletDirection = bulletDirection;
            this.bulletDamage = bulletDamage;
            this.weaponType = weaponType;
            extraWeaponInfo = null;

        }

        public BulletSpawnInfo(float bulletDamage, WeaponType weaponType, SniperWeaponInfo sniperWeaponInfo)
        {
            bulletPos = Vector2.zero;
            bulletDirection = Vector2.zero;
            this.bulletDamage = bulletDamage;
            this.weaponType = weaponType;
            extraWeaponInfo = sniperWeaponInfo;
        }
    }


    public class Bullet : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletActive;
        [SerializeField] private GameEvent onKnockbackTriggered;
        [SerializeField] private Rect currentBounds;

        public Vector2 direction;
        [SerializeField] private Vector2 velocity;
        public Vector2 bulletPos;

        [SerializeField] private float bulletSpeed;
        private float damage;
        #endregion

        // Update is called once per frame
        void Update()
        {
            
        }

        private void FixedUpdate()
        {
            // Move the bullet
            MoveBullet();
        }

        /// <summary>
        /// Move the bullet in its direction
        /// </summary>
        private void MoveBullet()
        {
            // Add velocity to bullet position
            bulletPos += velocity;

            transform.position = bulletPos;

            // Kill the bullet if out of the bounds
            if (bulletPos.x < currentBounds.xMin || bulletPos.x > currentBounds.xMax || bulletPos.y < currentBounds.yMin || bulletPos.y > currentBounds.yMax)
            {
                OnDeath();
            }

            bulletPos = transform.position;
        }

        /// <summary>
        /// Set the transform position to the bullet pos
        /// </summary>
        public void SetPos()
        {
            transform.position = bulletPos;
        }

        public void ActivateBullet()
        {
            onBulletActive.Raise(this, currentBounds);
        }

        public void SetVelocity()
        {
            // Set velocity
            velocity = direction * bulletSpeed;
        }

        /// <summary>
        /// On death, set the gameobject to inactive
        /// </summary>
        private void OnDeath()
        {
            gameObject.SetActive(false);
        }

        public void UpdateBounds(Component sender, object data)
        {
            if (data is Rect)
            {
                currentBounds = (Rect)data;
            }
        }

        public void SetBulletDamage(float newDamage)
        {
            damage = newDamage;
        }

        /// <summary>
        /// Checks for collision between this bullet's box collider and other colliders
        /// </summary>
        /// <param name="collision"></param>
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<EnemyController>())
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);

                // trigger knockback event
                onKnockbackTriggered.Raise(this, collision.gameObject);

                gameObject.SetActive(false);
                return;
            }

            if (collision.gameObject.GetComponent<TilemapRenderer>())
            {
                if (collision.gameObject.GetComponent<TilemapRenderer>().sortingLayerName == "Wall Tiles")
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

}