using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;

    Vector2 inputs;
    Vector3 velocity;
    Vector3 axisX;
    Vector3 axisZ;

    CharacterController controller;

    // Might add running later
    public float movSpeed = 0.025f;

    // Start is called before the first frame update
    void Start()
    {
        axisX.x = 1;
        axisZ.z = 1;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Movement();
    }

    void Movement()
    {
        Vector2 inputNormalized = inputs;
        velocity = (axisZ * inputNormalized.y + axisX * inputNormalized.x) * movSpeed;

        controller.Move(velocity);
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
    }
}
