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
    public class MinigunWeapon : Weapon
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletFired;
        #endregion

        // Update is called once per frame
        void Update()
        {

        }

        public override void BasicAttack(Component sender, object data)
        {
            // If the player cannot basic attack, return
            if (!canBasic) return;

            // Check if the data is the correct type
            if (data is Vector2)
            {
                // Create a bullet and raise the OnBulletFired event
                Vector2 bulletDirection = ((Vector2)data - (Vector2)transform.position).normalized;
                Vector2 bulletPos = (Vector2)transform.position;
                onBulletFired.Raise(this, new BulletSpawnInfo(bulletPos, bulletDirection, weaponStats.GetStat(WeaponStat.basicDamage), WeaponType.Minigun));

                // Start cooldown
                canBasic = false;
                StartCoroutine(ReduceCooldown(WeaponStat.basicReload, false));
            }
        }

        public override void SpecialAttack(Component sender, object data)
        {
            // if the player cannot special attack, return
            if (!canSpecial) return;

            if (data is Vector2)
            {
                // Do special
            }

        }
    }
}
