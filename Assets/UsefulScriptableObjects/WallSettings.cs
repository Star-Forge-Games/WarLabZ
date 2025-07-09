using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WallSettings", menuName = "Scriptable Objects/WallSettings")]
public class WallSettings : ScriptableObject
{
    [Serializable]
    public struct WallData
    {
        public int cost;
        public int hp;
        public float width;
    }

    public WallData[] wallLevels;
}