using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBaisc : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rigidbody;

    Vector3 movement;
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
