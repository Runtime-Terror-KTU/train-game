using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
    public enum CollectibleType
    {
        Health,
        Ammo
    }

    [System.Serializable]
    public class CollectibleData
    {
        public string id;
        public CollectibleType collectibleType;
        public Vector3 position;
        public Quaternion rotation;

    }
