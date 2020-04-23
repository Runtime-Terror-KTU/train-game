using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;
    public Interaction interaction;
    Vector2 inputs;
    float angle;
    public Rigidbody rigidbody;
    public Animator animator;
    public float movSpeed = 6f;
    public float backSpeed = 3f;
    public GunLogic gunLogic;
    Camera camera = Camera.main;
    string direction;

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
        Vector3 inputNormalized = new Vector3(inputs.x, 0, inputs.y);
        inputNormalized.Normalize();
        float test = Mathf.Abs(inputs.x) + Mathf.Abs(inputs.y);
        animator.SetFloat("Movement", test);

        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y, rot.z);
        var rotation = Quaternion.Euler(rot);
        Vector3 axis;
        rotation.ToAngleAxis(out angle, out axis);
        if (angle > 22.5 && angle <= 67.5)
            direction = "UpRight";
        else if (angle > 67.5 && angle <= 112.5)
            direction = "Right";
        else if (angle > 112.5 && angle <= 157.5)
            direction = "DownRight";
        else if (angle > 157.5 && angle <= 202.5)
            direction = "Down";
        else if (angle > 202.5 && angle <= 247.5)
            direction = "DownLeft";
        else if (angle > 247.5 && angle <= 292.5)
            direction = "Left";
        else if (angle > 292.5 && angle <= 337.5)
            direction = "UpLeft";
        else
            direction = "Up";

        switch (direction)
        {
            case "Right":
                if(inputNormalized.x >= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "Down":
                if (inputNormalized.z <= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "Left":
                if (inputNormalized.x <= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "Up":
                if (inputNormalized.z >= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
                //
            case "DownRight":
                if (inputNormalized.x >= 0 && inputNormalized.z <= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "DownLeft":
                if (inputNormalized.x <= 0 && inputNormalized.z <= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "UpRight":
                if (inputNormalized.x >= 0 && inputNormalized.z >= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
            case "UpLeft":
                if (inputNormalized.x <= 0 && inputNormalized.z >= 0)
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * movSpeed * Time.fixedDeltaTime);
                else
                    rigidbody.MovePosition(rigidbody.position + inputNormalized * backSpeed * Time.fixedDeltaTime);
                break;
        }
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
