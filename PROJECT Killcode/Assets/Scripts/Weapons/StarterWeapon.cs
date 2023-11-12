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
    public class StarterWeapon : Weapon
    {
        #region FIELDS
        [SerializeField] private GameEvent onBulletFired;
        [SerializeField] GameObject slashHitbox;
        [SerializeField] private float slashDuration;
        [SerializeField] private float slashSpeed;
        #endregion

        // Update is called once per frame
        void Update()
        {
            
        }

        public IEnumerator SlashAttack(Vector2 startPosition, Vector2 endPosition)
        {
            // Enable the hitbox.
            slashHitbox.SetActive(true);

            float elapsedTime = 0f;

            while (elapsedTime < slashDuration)
            {
                // Calculate the position along the arc path based on time.
                float t = elapsedTime / slashDuration;
                Vector2 newPosition = Vector2.Lerp(startPosition, endPosition, t);
                Debug.Log(newPosition);

                // Move the hitbox to the new position.
                slashHitbox.transform.position = newPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Disable the hitbox after the slash is complete.
            slashHitbox.SetActive(false);
        }

        public override void BasicAttack(Component sender, object data)
        {
            // If the player cannot basic attack, return
            if (!canBasic) return;

            // Check if the data is the correct type
            if(data is Vector2)
            {
                // Create a bullet and raise the OnBulletFired event
                Vector2 bulletDirection = ((Vector2)data - (Vector2)transform.position).normalized;
                Vector2 bulletPos = (Vector2)transform.position;
                onBulletFired.Raise(this, new BulletSpawnInfo(bulletPos, bulletDirection, weaponStats.GetStat(WeaponStat.basicDamage), WeaponType.Starter));

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
                Vector2 startPosition = transform.position - transform.right;
                Vector2 endPosition = transform.position + transform.right;

                StartCoroutine(SlashAttack(startPosition, endPosition));
            }
            
        }
    }
}
