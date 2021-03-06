﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Text healthText;
    public Player player;
    public Animation anim;
    private float health;
    // Update is called once per frame
    private void Start()
    {
        health = player.Health;
    }
    void FixedUpdate()
    {
        if(player.Health>=70)
        {
            healthText.color = Color.green;
        }
        else if (player.Health<70&&player.Health>=30)
        {
            healthText.color = Color.yellow;
        }
        else if(player.Health<30)
        {
            healthText.color = Color.red;
        }

        if(health > player.Health || health < player.Health)
        {
            anim.Play();
        }



        healthText.text = player.Health.ToString();
        health = player.Health;
    }
}
