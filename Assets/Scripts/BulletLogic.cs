using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{

    public float speed = 5f;
    public float lifeTime = 1.3f;
    public float damage = 10f;
    private Enemy enemy;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(gameObject);
            enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
        }
    }
}
