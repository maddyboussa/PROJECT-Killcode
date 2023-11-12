using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Killcode.Levels
{
    [CreateAssetMenu(menuName = "Level Data")]
    public class LevelData : ScriptableObject
    {
        public SerializedDictionary<int, List<Vector3>> wavePositions;
    }
}
