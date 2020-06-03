using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public PlayerData player;
    public List<EnemyData> enemies;
    public List<CollectibleData> collectibles;
}
