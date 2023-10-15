using AYellowpaper.SerializedCollections;
using Killcode.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Killcode.Weapons
{
    public enum WeaponStat
    {
        basicDamage,
        specialDamage,
        basicReload,
        specialReload
    }

    [CreateAssetMenu(menuName = "Weapon Stats")]
    public class WeaponStats : ScriptableObject
    {
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<WeaponStat, float> stats = new SerializedDictionary<WeaponStat, float>();

        /// <summary>
        /// Attempt to get the value of a stat
        /// </summary>
        /// <param name="stat"></param>
        /// <returns>The value of a stat, or 0 if the stat doesn't exist</returns>
        public float GetStat(WeaponStat stat)
        {
            if (stats.TryGetValue(stat, out float value))
            {
                return value;
            }
            else
            {
                Debug.LogError($"No stat value found for {stat} on {this.name}");
                return 0;
            }
        }

        /// <summary>
        /// Upgrade the value of a stat
        /// </summary>
        /// <param name="stat">The stat to upgrade</param>
        /// <param name="upgradeAmount">The amount to uprade the stat by</param>
        public void UpgradeStat(WeaponStat stat, float upgradeAmount)
        {
            if (stats.TryGetValue(stat, out float value))
            {
                stats[stat] += upgradeAmount;
            }
            else
            {
                Debug.LogError($"No stat value found for {stat} on {this.name}");
            }
        }
    }
}
