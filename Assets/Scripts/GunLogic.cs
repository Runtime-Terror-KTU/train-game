using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{

    public bool isFiring;
    public BulletLogic bullet;
    public float shootDelay;
    private float shotTimer;
    public Transform firePoint;

    public float bulletSpeed;
    public float bulletDamage;
    public float bulletLifeTime = 1.1f;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;


    void Start()
    {
        currentAmmo = maxAmmo;
    }


    void Update()
    {
        if (isReloading)
            return;

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
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
                newBullet.damage = bulletDamage;
                newBullet.lifeTime = bulletLifeTime;
                currentAmmo--;
            }
        }
        else
        {
            shotTimer = 0;
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;
    }
}
