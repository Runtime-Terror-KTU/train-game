using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{
    public bool isFiring = false;
    public bool isReloading = false;
    public PlayerControls PlayerControls;
    public WeaponSystem weaponSystem;
    public float shootDelay;
    private float shotTimer;
    public Transform firePoint;

    public float bulletDamage;
    public float bulletRange = 150f;
    public float bulletForce = 100f;

    public int magazineSize;
    public int currentAmmo;
    public int reserveAmmo;

    public float reloadTime = 1f;

    public bool isPistol;
    public bool isAk;
    public bool isSvd;



    void Start()
    {

    }

    void Update()
    {
        GetAmmo();

        isFiring = PlayerControls.isFiring;
        if (!isReloading)
        {
            if (reserveAmmo != 0)
            {
                if (PlayerControls.isReloading && reserveAmmo != 0 && currentAmmo != magazineSize || currentAmmo == 0)
                {
                    StartCoroutine(Reload());
                    return;
                }
            }
            else
            {
                GetAmmo();
                isReloading = false;
            }

        }

        if (!isReloading)
        {
            if (PlayerControls.isFiring && currentAmmo != 0)
            {
                Fire();
            }
            else
            {
                shotTimer = 0;
            }
        }

        UpdateAmmo();
    }

    void Fire()
    {
        shotTimer -= Time.deltaTime;
        if(shotTimer <=0)
        {
            shotTimer = shootDelay;

            RaycastHit hit;
            Physics.Raycast(firePoint.transform.position, firePoint.transform.forward, out hit, bulletRange);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * bulletForce);
            }

            currentAmmo--;
            UpdateAmmo();
        }       
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        if (reserveAmmo - (magazineSize - currentAmmo) > 0)
        {
            reserveAmmo = reserveAmmo - (magazineSize - currentAmmo);
            currentAmmo = magazineSize;
        }
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }

        UpdateReserveAmmo();
        isReloading = false;
    }

    void GetAmmo()
    {
        if (isPistol)
            reserveAmmo = weaponSystem.pistol_ammo;

        if (isAk)
            reserveAmmo = weaponSystem.ak_ammo;

        if (isSvd)
            reserveAmmo = weaponSystem.svd_ammo;
    }

    void UpdateReserveAmmo()
    {
        if (isPistol)
            weaponSystem.pistol_ammo = reserveAmmo;

        if (isAk)
            weaponSystem.ak_ammo = reserveAmmo;

        if (isSvd)
            weaponSystem.svd_ammo = reserveAmmo;
    }

    void UpdateAmmo()
    {
        weaponSystem.currentAmmo = currentAmmo;
        weaponSystem.currentReserve = reserveAmmo;
    }
}
