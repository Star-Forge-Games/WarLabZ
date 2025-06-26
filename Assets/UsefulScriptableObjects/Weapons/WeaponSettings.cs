using System;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponSettings", menuName = "Scriptable Objects/WeaponSettings")]
public class WeaponSettings : ScriptableObject
{
    [Serializable]
    public struct Level
    {
        public int cost;
        public int damage;
        public float aspd;
        public float crit;
        public float critChance;
    }
    public Level[] levels;
}
