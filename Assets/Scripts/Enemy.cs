using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Health = 100;
    public float MovementSpeed = 4f;
    public float MeleeDamage = 25f;
    public float MeleeSpeed = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
