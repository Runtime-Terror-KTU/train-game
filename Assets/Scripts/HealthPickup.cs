using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private Player player;
    public float AddHealth = 25;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
            player = other.gameObject.GetComponent<Player>();
            player.GiveHealth(AddHealth);
        }
    }
}
