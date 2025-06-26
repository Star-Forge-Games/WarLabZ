using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Scriptable Objects/WeaponSettings")]
public class WeaponSettings : ScriptableObject
{
    [Serializable]
    public struct Level
    {
        public int cost;
        public int damageBuff;
        public float aspdBuff;
        public float critBuff;
        public float critChanceBuff;
    }
    public Level[] levels;
}
