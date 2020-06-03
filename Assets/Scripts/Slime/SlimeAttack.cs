using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    Player player;
    public float Health = 50;
    public float DieTime = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Health <= 0)
        {
            FindObjectOfType<AudioManager>().Play("Death");
            FindObjectOfType<Animator>().SetTrigger("Death");
            Destroy(this.gameObject, DieTime);
        }
    }

    public void Attack()
    {
        player.TakeDamage(10);
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }
}
