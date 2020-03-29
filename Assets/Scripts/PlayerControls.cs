using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;
    Vector2 inputs;
    public Rigidbody rigidbody;
    public Animator animator;

    // Might add running speed later
    public float movSpeed = 0f;
    public GunLogic gunLogic;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 inputNormalized = new Vector3(inputs.x, 0, inputs.y);
        inputNormalized.Normalize();
        float test =  Mathf.Abs(inputs.x) + Mathf.Abs(inputs.y);
        animator.SetFloat("Movement", test);   
        rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
    }

    void GetInputs()
    {
        //Forwards Backwards controls
        //Forwards
        if (Input.GetKey(controls.forwards))
            inputs.y = 1;
        
        //Backwards
        if (Input.GetKey(controls.backwards))
        {
            //Cancels out each other
            if (Input.GetKey(controls.forwards))
                inputs.y = 0;
            else
                inputs.y = -1;
        }

        //FB idle
        if (!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
            inputs.y = 0;

        //Right Left controls
        //Right
        if (Input.GetKey(controls.right))
            inputs.x = 1;

        //Left
        if (Input.GetKey(controls.left))
        {
            //Cancels out each other
            if (Input.GetKey(controls.right))
                inputs.x = 0;
            else
                inputs.x = -1;
        }

        //LR idle
        if (!Input.GetKey(controls.left) && !Input.GetKey(controls.right))
            inputs.x = 0;

        //Left click
        if (Input.GetKey(controls.fire))
            gunLogic.isFiring = true;
        else
            gunLogic.isFiring = false;
    }
}
