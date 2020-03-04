using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Controls controls;

    [SerializeField]
    Vector2 inputs;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
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
                inputs.y = 0;
            else
                inputs.y = -1;
        }

        //LR idle
        if (!Input.GetKey(controls.left) && !Input.GetKey(controls.right))
            inputs.y = 0;
    }
}
