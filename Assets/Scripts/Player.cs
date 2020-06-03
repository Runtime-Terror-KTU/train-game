using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    CharacterController cc;
    public float Health;

    public Animator anim;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(Health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Death");
            anim.SetTrigger("Fade");
            StartCoroutine(Koroutina());

        }
        if(cc.isGrounded == true && cc.velocity.magnitude > 1f && FindObjectOfType<AudioManager>().isPlaying("Footsteps") == false)
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
        }
        if(cc.velocity.magnitude < 1f && FindObjectOfType<AudioManager>().isPlaying("Footsteps") == true)
        {
            FindObjectOfType<AudioManager>().Stop("Footsteps");
        }
    }

    public void TakeDamage(float dmg)
    {
        if (Health > 0)
        {
            Health -= dmg;
            if (Health < 0)
                Health = 0;
            //if (FindObjectOfType<AudioManager>().isPlaying("Footsteps") == false)
            FindObjectOfType<AudioManager>().Play("Player Hit");
        }
    }

    public void GiveHealth(float health)
    {
        //if(Health < 100)
        //{
            Health += health;
        //}
        
    }

    IEnumerator Koroutina()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("TempScene");
    }
}
