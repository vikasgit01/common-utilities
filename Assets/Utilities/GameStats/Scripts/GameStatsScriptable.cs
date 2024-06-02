using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtills
{
    [CreateAssetMenu(fileName = "GameStatsScriptable", menuName = "ScriptableObjects/GameStats", order = 1)]
    public class GameStatsScriptable : ScriptableObject
    {
        [SerializeField] public bool frameTime;
        [SerializeField] public bool gcMemory;
        [SerializeField] public bool systemMemory;
        [SerializeField] public bool drawCalls;
        [SerializeField] public bool frameRate;

        public bool GetFrameTime() => frameTime;
        public bool GetGCMemory() => gcMemory;
        public bool GetSystemMemory() => systemMemory;
        public bool GetDrawCalls() => drawCalls;
        public bool GetFrameRate() => frameRate;
    }
}