using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{

    public bool isFiring;
    public BulletLogic bullet;
    public float bulletSpeed;
    public float shootDelay;
    private float shotTimer;
    public Transform firePoint;

    void Start()
    {
        
    }


    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                shotTimer = shootDelay;
                BulletLogic newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as BulletLogic;
                newBullet.speed = bulletSpeed;
                //Destroy(bullet, 1.0f);
                //Destroy(newBullet, 1.0f);
                DestroyObject(newBullet, 1.0f);
            }
        }
        else
        {
            shotTimer = 0;
        }
    }
}
