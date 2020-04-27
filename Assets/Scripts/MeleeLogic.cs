using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    public PlayerControls playerControls;
    public WeaponSystem weaponSystem;
    public float hitDelay;
    private float hitTimer;
    public Transform hitPoint;

    public bool isHitting = false;
    public float hitDamage;
    public float hitRange = 150f;
    public float hitForce = 100f;

    void Start()
    {

    }

    void Update()
    {
        isHitting = playerControls.isFiring;

        if (playerControls.isFiring)
        {
            Hit();
        }
    }

    void Hit()
    {
        hitTimer -= Time.deltaTime;
        if (hitTimer <= 0)
        {
            hitTimer = hitDelay;

            RaycastHit hit;
            Physics.Raycast(hitPoint.transform.position, hitPoint.transform.forward, out hit, hitRange);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(hitDamage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }

        }
    }

}