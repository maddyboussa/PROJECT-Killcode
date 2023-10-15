using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Killcode.Core
{
    public enum Stat
    {
        hitPoints,
        maxHitPoints,
        maxSpeed,
        accelRate
    }

    [CreateAssetMenu(menuName = "Unit Stats")]
    public class Stats : ScriptableObject
    {
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<Stat, float> originalStats = new SerializedDictionary<Stat, float>();
        public SerializedDictionary<Stat, float> instanceStats = new SerializedDictionary<Stat, float>();

        /// <summary>
        /// Attempt to get the value of a stat
        /// </summary>
        /// <param name="stat"></param>
        /// <returns>The value of a stat, or 0 if the stat doesn't exist</returns>
        public float GetStat(Stat stat)
        {
            if(instanceStats.TryGetValue(stat, out float value))
            {
                return value;
            } else
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
        public void UpgradeStat(Stat stat, float upgradeAmount)
        {
            if(instanceStats.TryGetValue(stat, out float value))
            {
                instanceStats[stat] += upgradeAmount;
            } else
            {
                Debug.LogError($"No stat value found for {stat} on {this.name}");
            }
        }

        /// <summary>
        /// Set a stat to a certain value
        /// </summary>
        /// <param name="stat">The stat to upgrade</param>
        /// <param name="value">The value to change the stat to</param>
        public void SetStat(Stat stat, float value)
        {
            if (instanceStats.TryGetValue(stat, out float statValue))
            {
                instanceStats[stat] = value;
            }
            else
            {
                Debug.LogError($"No stat value found for {stat} on {this.name}");
            }
        }

        /// <summary>
        /// Reset stats to their original value
        /// </summary>
        public void ResetStats()
        {
            // Reset instance stats to the original values
            foreach(KeyValuePair<Stat, float> pair in instanceStats.ToList())
            {
                // Set each stat to the original stats value
                SetStat(pair.Key, originalStats[pair.Key]);
            }
        }
    }
}
