using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{

    public bool isReloading = false;
    public PlayerControls PlayerControls;
    public WeaponSystem weaponSystem;
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

    public bool isPistol;
    public bool isAk;
    public bool isSvd;
    


    void Start()
    {
        if (isPistol)
            reserveAmmo = weaponSystem.pistol_ammo;
        
        if (isAk)
            reserveAmmo = weaponSystem.ak_ammo;
        
        if (isSvd)
            reserveAmmo = weaponSystem.svd_ammo;
    }

    void Update()
    {   

        if (isReloading)
            return;

        if (PlayerControls.isReloading && reserveAmmo != 0 && currentAmmo != magazineSize || currentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(PlayerControls.isFiring && currentAmmo != 0)
        {
            Fire();
        }
        else
        {
            shotTimer = 0;
        }

        UpdateAmmo();
    }

    void Fire()
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

    void UpdateAmmo()
    {
        if (isPistol)
            weaponSystem.pistol_ammo = reserveAmmo;

        if (isAk)
            weaponSystem.ak_ammo = reserveAmmo;

        if (isSvd)
            weaponSystem.svd_ammo = reserveAmmo;
    }
}
