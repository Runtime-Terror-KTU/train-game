using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator anim;

    public float Health = 100;
    public float MovementSpeed = 4f;
    public float MeleeDamage = 25f;
    public float MeleeSpeed = 0.5f;
    public float DieTime = 3f;


    void Start()
    {
        setRigidbodyState(true);
        setColliderState(false);
    }

    void Update()
    {
        if (GetComponent<EnemyAI>().isMoving && !GetComponent<EnemyAI>().isInRange)
        {
            anim.SetBool("Moving", true);
        }
        else
            anim.SetBool("Moving", false);

        if(Health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
    }

    void Die()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        Destroy(gameObject, DieTime);
    }

    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach(Collider cs in colliders)
        {
            cs.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
