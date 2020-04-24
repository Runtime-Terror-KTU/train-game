using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;
    public Interaction interaction;
    [SerializeField]
    public Vector3 inputs;
    float angle;
    bool walking;
    public Rigidbody rigidbody;
    public Animator animator;
    public float movSpeed = 6f;
    public float sideSpeed = 5f;
    public float backSpeed = 3f;
    public GunLogic gunLogic;
    string direction;

    public bool isFiring;
    public bool isReloading;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenu.OnPause)
        {

        }
        else
        {
            GetInputs();
        }
        
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (walking)
        {
            movSpeed = 3f;
            sideSpeed = 3f;
            backSpeed = 3f;
        }
        else
        {
            movSpeed = 6;
            sideSpeed = 5f;
            backSpeed = 3f;
        }
        Vector3 inputNormalized = new Vector3(inputs.x, 0, inputs.z);
        inputNormalized.Normalize();
        float test = Mathf.Abs(inputs.x) + Mathf.Abs(inputs.z);
        animator.SetFloat("Movement", test);

        if(inputNormalized.z < 0)
            rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
        else if(inputNormalized.z > 0)
            rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
        else
            rigidbody.MovePosition(rigidbody.position + inputNormalized * sideSpeed * Time.fixedDeltaTime);
    }

    void GetInputs()
    {
        if (Input.GetKey(controls.jump))
            inputs.y = 1;
        else
            inputs.y = 0;

        if (Input.GetKey(controls.walk))
            walking = true;
        else
            walking = false;

        if (Input.GetKey(controls.forwards))
            inputs.z = 1;
       
        if (Input.GetKey(controls.backwards))
        {
            if (Input.GetKey(controls.forwards))
                inputs.z = 0;
            else
                inputs.z = -1;
        }

        if (!Input.GetKey(controls.forwards) && !Input.GetKey(controls.backwards))
            inputs.z = 0;

        if (Input.GetKey(controls.right))
            inputs.x = 1;

        if (Input.GetKey(controls.left))
        {
            if (Input.GetKey(controls.right))
                inputs.x = 0;
            else
                inputs.x = -1;
        }

        if (!Input.GetKey(controls.left) && !Input.GetKey(controls.right))
            inputs.x = 0;

        if (Input.GetKey(controls.fire))
            isFiring = true;
        else
            isFiring = false;

        if (Input.GetKey(controls.reload))
        {
            isReloading = true;
        }
        else
        {
            isReloading = false;
        }

        if (Input.GetKey(controls.use))
        {
            interaction.Interact();
        }

    }
}
