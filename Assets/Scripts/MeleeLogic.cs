using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLogic : MonoBehaviour
{
    public bool isHitting = false;
    public PlayerControls PlayerControls;
    public WeaponSystem weaponSystem;
    public float hitDelay;
    private float hitTimer;
    public Transform hitPoint;

    public float hitDamage;
    public float hitRange = 150f;
    public float hitForce = 100f;

    void Update()
    {
        isHitting = PlayerControls.isFiring;

        if (PlayerControls.isFiring)
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
