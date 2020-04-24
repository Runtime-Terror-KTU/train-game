using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    Rigidbody rigidbody;
    CharacterController controller;
    public Controls controls;
    public Interaction interaction;

    [SerializeField]
    public Vector3 inputs;
    //walking
    
    bool walking, jump;
    public bool isGrounded { get; private set; }
    bool isJumping;
    Vector3 jumpDirection;
    Vector3 groundNormal;
    Vector3 characterVelocity;


    public Animator animator;
    public float movSpeed = 6f;
    public float sideSpeed = 5f;
    public float backSpeed = 3f;
    public bool isFiring;
    public bool isReloading;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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

        //Vector3 move = new Vector3(inputNormalized.x, 0f, inputNormalized.z);
        //Vector3 worldInput = transform.TransformVector(move);

        //controller.SimpleMove(inputNormalized * movSpeed);
        float vertInput = Input.GetAxis("Vertical");
        float horizInput = Input.GetAxis("Horizontal");

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 sideMovement = transform.right * horizInput;

        controller.SimpleMove(Vector3.ClampMagnitude(forwardMovement + sideMovement, 1.0f) * movSpeed);

        
    }


    void GetInputs()
    {
        jump = Input.GetKey(controls.jump);

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
