using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyType
{
    Melee,
    Shooter,
}

[System.Serializable]
public class EnemyData
{
    public EnemyType enemyType;
    public Vector3 position;
    public Quaternion rotation;

}
