using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Health;

    void Start()
    {
        
    }

    void Update()
    {
        if(Health <= 0)
        {
            Debug.Log("U heff died");
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

    public void GiveHealth(float health)
    {
        //if(Health < 100)
        //{
            Health += health;
        //}
        
    }
}
