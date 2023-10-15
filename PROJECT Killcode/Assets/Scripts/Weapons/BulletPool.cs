using Killcode.Events;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Killcode.Weapons
{
    public class BulletPool : MonoBehaviour
    {
        #region FIELDS
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private int poolSize = 5;
        private List<GameObject> bulletPool;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // Create the bullet pool and populate it
            bulletPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                // Instantiate bullets equal to the poolsize and set them as inactive
                GameObject bullet = Instantiate(bulletPrefab);
                bulletPool.Add(bullet);
                bullet.SetActive(false);
            }
        }

        public void ActivateBullet(Component sender, object data)
        {
            foreach (GameObject bullet in bulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    // Set the bullet as active
                    bullet.SetActive(true);

                    // Check if the correct data is being sent
                    if(data is BulletSpawnInfo)
                    {
                        // Set the bullet's spawn info
                        BulletSpawnInfo spawnInfo = (BulletSpawnInfo)data;
                        bullet.GetComponent<Bullet>().ActivateBullet();
                        
                        bullet.GetComponent<Bullet>().bulletPos = spawnInfo.bulletPos;
                        bullet.GetComponent<Bullet>().direction = spawnInfo.bulletDirection;
                        bullet.GetComponent<Bullet>().SetBulletDamage(spawnInfo.bulletDamage);
                        bullet.GetComponent<Bullet>().SetPos();
                    }

                    return;
                }
            }
        }
    }
}
