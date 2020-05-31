using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float Health;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(Health <= 0)
        {
            Debug.Log("U heff died");
        }
        if(cc.isGrounded == true && cc.velocity.magnitude > 2f && FindObjectOfType<AudioManager>().isPlaying("Footsteps") == false)
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
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
