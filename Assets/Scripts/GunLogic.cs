using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int magazineSize;
    public int currentAmmo;
    public int reserveAmmo;

    public float reloadTime = 1f;
    public bool isReloading = false;
    public bool manualReload = false;


    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
            isFiring = true;
        else
            isFiring = false;

        //Reload
        if (Input.GetKey(KeyCode.R))
        {
            manualReload = true;
        }
        else
        {
            manualReload = false;
        }

        if (isReloading)
            return;

        if (manualReload && reserveAmmo != 0 && currentAmmo != magazineSize || currentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Fire();

    }

    void Fire()
    {
        if (isFiring && currentAmmo != 0)
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

        if(reserveAmmo - (magazineSize - currentAmmo) > 0)
        {   
            reserveAmmo = reserveAmmo - (magazineSize - currentAmmo);
            currentAmmo = magazineSize;
        }
        else 
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }

        isReloading = false;
    }
}
