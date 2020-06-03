using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    Player player;
    public Animator anim;
    public float Health = 30;
    public float DieTime = 5f;
    // Start is called before the first frame update

    bool dead = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (Health <= 0)
        {
            if (dead != true)
            {
                dead = true;
                StartCoroutine(Die());
            }
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

    IEnumerator Die()
    {
        FindObjectOfType<AudioManager>().Play("Death");
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(3); 
        Destroy(gameObject);
    }
}
