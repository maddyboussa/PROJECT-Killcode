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
        [SerializeField] private GameObject sniperPrefab;

        [SerializeField] private int standardSize = 5;
        [SerializeField] private int sniperPoolSize = 5;
        private List<GameObject> standardBulletPool;
        private List<GameObject> sniperBulletPool;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // Create the standard bullet pool and populate it
            standardBulletPool = new List<GameObject>();
            for (int i = 0; i < standardSize; i++)
            {
                // Instantiate bullets equal to the poolsize and set them as inactive
                GameObject bullet = Instantiate(bulletPrefab);
                standardBulletPool.Add(bullet);
                bullet.SetActive(false);
            }

            // Create the sniper bullet pool and populate it
            sniperBulletPool = new List<GameObject>();
            for (int i = 0; i < sniperPoolSize; i++)
            {
                // Instantiate bullets equal to the poolsize and set them as inactive
                GameObject bullet = Instantiate(sniperPrefab);
                sniperBulletPool.Add(bullet);
                bullet.SetActive(false);
            }
        }

        public void ActivateBullet(Component sender, object data)
        {
            if (!(data is BulletSpawnInfo))
            {
                return;
            }

            // Set the bullet's spawn info
            BulletSpawnInfo spawnInfo = (BulletSpawnInfo)data;

            switch (spawnInfo.weaponType)
            {
                case WeaponType.Starter:
                    foreach (GameObject bullet in standardBulletPool)
                    {
                        if (!bullet.activeInHierarchy)
                        {
                            // Set the bullet as active
                            bullet.SetActive(true);

                            // Activate the bullet component
                            bullet.GetComponent<Bullet>().ActivateBullet();

                            // Set bullet variables
                            bullet.GetComponent<Bullet>().bulletPos = spawnInfo.bulletPos;
                            bullet.GetComponent<Bullet>().direction = spawnInfo.bulletDirection;
                            bullet.GetComponent<Bullet>().SetBulletDamage(spawnInfo.bulletDamage);
                            bullet.GetComponent<Bullet>().SetPos();
                            bullet.GetComponent<Bullet>().SetVelocity();

                            return;
                        }
                    }
                    break;

                case WeaponType.Sniper:
                    foreach (GameObject bullet in sniperBulletPool)
                    {
                        if (!bullet.activeInHierarchy)
                        {
                            // Set the bullet as active
                            bullet.SetActive(true);

                            // Activate the bullet component
                            bullet.GetComponent<SniperBullet>().ActivateBullet();

                            // Check for extra info
                            if (spawnInfo.extraWeaponInfo is SniperWeaponInfo)
                            {
                                SniperWeaponInfo weaponInfo = (SniperWeaponInfo)spawnInfo.extraWeaponInfo;

                                // Set sniper bullet information
                                bullet.GetComponent<SniperBullet>().SetBullet(weaponInfo);
                                bullet.GetComponent<SniperBullet>().SetBulletDamage(spawnInfo.bulletDamage);
                            }

                            return;
                        }
                    }
                    break;

                case WeaponType.Shotgun:
                    for (int i = 0; i < 5; i++)
                    {
                        foreach (GameObject bullet in standardBulletPool)
                        {
                            if (!bullet.activeInHierarchy)
                            {
                                // Set the bullet as active
                                bullet.SetActive(true);

                                // Activate the bullet component
                                bullet.GetComponent<Bullet>().ActivateBullet();

                                // Set bullet variables
                                bullet.GetComponent<Bullet>().bulletPos = spawnInfo.bulletPos;

                                // Convert direction to angle
                                float angle = Mathf.Atan2(spawnInfo.bulletDirection.y, spawnInfo.bulletDirection.x);
                                // Change angle based on bullet num
                                // 2 are always neg, 1 is zero, and 2 are always pos
                                angle += (i - 2) * Random.Range(0.001f, 0.1f);

                                // Convert back to vec2
                                bullet.GetComponent<Bullet>().direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


                                bullet.GetComponent<Bullet>().SetBulletDamage(spawnInfo.bulletDamage);
                                bullet.GetComponent<Bullet>().SetPos();
                                bullet.GetComponent<Bullet>().SetVelocity();

                                break;
                            }
                        }
                    }
                    break;

                case WeaponType.Minigun:
                        foreach (GameObject bullet in standardBulletPool)
                        {
                            if (!bullet.activeInHierarchy)
                            {
                                // Set the bullet as active
                                bullet.SetActive(true);

                                // Activate the bullet component
                                bullet.GetComponent<Bullet>().ActivateBullet();

                                // Set bullet variables
                                bullet.GetComponent<Bullet>().bulletPos = spawnInfo.bulletPos;

                                // Convert direction to angle
                                float angle = Mathf.Atan2(spawnInfo.bulletDirection.y, spawnInfo.bulletDirection.x);
                                // Change angle randomly to simulate spray
                                angle += Random.Range(-0.08f, 0.08f);

                                // Convert back to vec2
                                bullet.GetComponent<Bullet>().direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));


                                bullet.GetComponent<Bullet>().SetBulletDamage(spawnInfo.bulletDamage);
                                bullet.GetComponent<Bullet>().SetPos();
                                bullet.GetComponent<Bullet>().SetVelocity();

                                return;
                            }
                        }
                    break;
            }


        }
    }
}
