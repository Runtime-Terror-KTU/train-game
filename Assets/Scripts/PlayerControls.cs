using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;
    public Interaction interaction;
    [SerializeField]
    public Vector2 inputs;
    //temp
    [SerializeField]
    public Vector3 temp;
    //
    public Rigidbody rigidbody;
    public Animator animator;
    public float movSpeed = 6f;
    public float backSpeed = 3f;
    public GunLogic gunLogic;
    Camera camera = Camera.main;


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

    //Original
    //void Movement()
    //{
    //    Vector3 inputNormalized = new Vector3(inputs.x, 0, inputs.y);
    //    inputNormalized.Normalize();
    //    float test =  Mathf.Abs(inputs.x) + Mathf.Abs(inputs.y);
    //    animator.SetFloat("Movement", test);   
    //    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
    //}

    void Movement()
    {
        Vector3 inputNormalized = new Vector3(inputs.x, 0, inputs.y);
        inputNormalized.Normalize();
        float test = Mathf.Abs(inputs.x) + Mathf.Abs(inputs.y);
        animator.SetFloat("Movement", test);

        temp = new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
        //relative quarter to mouse
        if (inputNormalized.z < 0)
            rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
        else
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

        //Reload
        if (Input.GetKey(controls.reload))
        {
            gunLogic.manualReload = true;
        }
        else
        {
            gunLogic.manualReload = false;
        }

        //Interaction e
        if (Input.GetKey(controls.use))
        {
            interaction.Interact();
        }

    }
}
