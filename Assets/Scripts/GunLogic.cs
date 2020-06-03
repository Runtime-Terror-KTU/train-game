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
    private ParticleSystem mzl;
    public Camera cam;
    public Transform firePoint;
    public Material material;
    Animator anim;

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
        mzl = GetComponentInChildren<ParticleSystem>();
        anim = this.GetComponent<Animator>();
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

        if (PlayerControls.isMoving)
        {
            anim.SetBool("Run", true);
        }
        if (!PlayerControls.isMoving)
        {
            anim.SetBool("Run", false);
        }

        UpdateAmmo();
    }

    void Fire()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0)
        {
            anim.SetTrigger("Shoot");
            shotTimer = shootDelay;

            if (mzl != null)
                mzl.Play();

            if (isPistol)
            {
                FindObjectOfType<AudioManager>().Play("Pistol Shot");
            }
            if (isAk)
            {
                FindObjectOfType<AudioManager>().Play("AK Shot");
            }
            if (isSvd)
            {
                FindObjectOfType<AudioManager>().Play("SVD Shot");
            }

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, bulletRange))
            {
                CreateTracer(firePoint.position, hit.point, Color.yellow, 0.1f);
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(bulletDamage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * bulletForce);
                }

                SlimeAttack slimeEnemy = hit.transform.GetComponent<SlimeAttack>();
                if (slimeEnemy != null)
                {
                    slimeEnemy.TakeDamage(bulletDamage);
                }

            }

            currentAmmo--;
            UpdateAmmo();
        }
        else
        {
            if (mzl != null)
                mzl.Stop();
        }    
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        weaponSystem.isReloading = true;

        anim.SetTrigger("Reload");

        if (mzl != null)
            mzl.Stop();

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

        if (isPistol)
        {
            FindObjectOfType<AudioManager>().Play("Pistol Reload");
        }
        if (isAk)
        {
            FindObjectOfType<AudioManager>().Play("AK Reload");
        }
        if (isSvd)
        {
            FindObjectOfType<AudioManager>().Play("SVD Reload");
        }

        UpdateReserveAmmo();
        isReloading = false;
        weaponSystem.isReloading = false;
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

    void CreateTracer(Vector3 fromPos, Vector3 targetPos, Color color, float duration)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = fromPos;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        lr.material.color = color;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, fromPos);
        lr.SetPosition(1, targetPos);
        GameObject.Destroy(myLine, duration);
    }
}
