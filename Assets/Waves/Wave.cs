using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Scriptable Objects/Wave")]
public class Wave : ScriptableObject
{
    [Serializable]
    public struct WavePart
    {
        public GameObject zombiePrefab;
        public int amount;
        public float interval;
        public float nextPartDelay;
    }

    public WavePart[] parts;
}
