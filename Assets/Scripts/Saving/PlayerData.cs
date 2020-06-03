using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public int[] ammo = new int[3];
    public int[] reserveAmmo = new int[3];
    public bool[] weaponsUnlocked = new bool[3];
    public int currentWeapon;
    public Vector3 position;
    public Quaternion rotation;
}
