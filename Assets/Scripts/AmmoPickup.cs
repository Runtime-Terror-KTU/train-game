using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private WeaponSystem weaponSystem;
    private System.Random rand = new System.Random();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Ammo pickup");
            Destroy(gameObject);
            weaponSystem = other.gameObject.GetComponentInChildren<WeaponSystem>();
            weaponSystem.pistol_ammo += rand.Next(5, 10);
            weaponSystem.ak_ammo += rand.Next(15, 30);
            weaponSystem.svd_ammo += rand.Next(2, 5);
        }
    }
}
