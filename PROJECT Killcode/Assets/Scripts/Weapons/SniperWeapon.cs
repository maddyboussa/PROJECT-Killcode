using Killcode.Core;
using Killcode.Events;
using Killcode.Weapons;
using Killcode.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Killcode.Weapons
{
    public struct SniperWeaponInfo
    {
        public Vector2 startingPoint;
        public float distance;
        public float angle;

        public SniperWeaponInfo(Vector2 startingPoint, float distance, float angle)
        {
            this.startingPoint = startingPoint;
            this.distance = distance;
            this.angle = angle;
        }
    }
    public class SniperWeapon : Weapon
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletFired;
        [SerializeField] private LayerMask wallMask;
        #endregion

        public override void BasicAttack(Component sender, object data)
        {
            // If the player cannot basic attack, return
            if (!canBasic) return;

            // Check if the data is the correct type
            if(data is Vector2)
            {
                // Calculate the direction from transform.position to cursorPosition
                Vector2 direction = ((Vector2)data - (Vector2)transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, float.MaxValue, wallMask);

                if (hit)
                {
                    // Retrieve endpoint
                    Vector2 endPoint = hit.point;

                    // Calculate the midpoint between transform.position and cursorPosition
                    Vector2 midpoint = ((Vector2)transform.position + endPoint) * 0.5f;

                    // Calculate the distance between the two positions
                    float distance = (endPoint - (Vector2)transform.position).magnitude;

                    // Calculate the angle (in degrees) to rotate the square sprite
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Create a struct to hold information
                    SniperWeaponInfo weaponInfo = new SniperWeaponInfo(midpoint, distance, angle);

                    // Create a bullet and raise the OnBulletFired event
                    onBulletFired.Raise(this, new BulletSpawnInfo(weaponStats.GetStat(WeaponStat.basicDamage), WeaponType.Sniper, weaponInfo));
                }

                // Start cooldown
                canBasic = false;
                StartCoroutine(ReduceCooldown(WeaponStat.basicReload, false));
            }
        }

        public override void SpecialAttack(Component sender, object data)
        {
            // if the player cannot special attack, return
            if (!canSpecial) return;

            if(data is Vector2)
            {
                // Do special
            }
            
        }
    }
}
