using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class Wave : ScriptableObject
{
    [Serializable]
    public struct WavePart
    {
        public int delay;
        public GameObject zombiePrefab;
        public int amount;
        public float interval;
        public float hpMultiplier;
        //public float approximateTime;
        //public float nextPartDelay;
    }

    public WavePart[] parts;
    public float nextWaveDelay;

}
