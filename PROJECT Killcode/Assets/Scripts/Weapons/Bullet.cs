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

        public BulletSpawnInfo(Vector2 bulletPos, Vector2 bulletDirection, float bulletDamage)
        {
            this.bulletPos = bulletPos;
            this.bulletDirection = bulletDirection;
            this.bulletDamage = bulletDamage;
        }
    }


    public class Bullet : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletActive;
        [SerializeField] private Rect currentBounds;
        private Rigidbody2D rb;

        public Vector2 direction;
        [SerializeField] private Vector2 velocity;
        public Vector2 bulletPos;

        [SerializeField] private float bulletSpeed;
        private float damage;
        #endregion

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

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
            // Calculate velocity and add force
            velocity = direction * bulletSpeed;

            rb.AddForce(velocity, ForceMode2D.Force);

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