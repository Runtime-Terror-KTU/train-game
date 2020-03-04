using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBaisc : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rigidbody;
    public GunLogic gun;

    Vector3 movement;
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButtonDown(0))
        {
            gun.isFiring = true;
        }

        if(Input.GetMouseButtonUp(0))
        {
            gun.isFiring = false;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
