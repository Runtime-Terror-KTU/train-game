using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
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
    Vector3 m_CharacterVelocity;


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
        Vector3 move = new Vector3(inputNormalized.x, 0f, inputNormalized.z);
        Vector3 worldInput = transform.TransformVector(move);
        
    }

    void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance = isGrounded ? (m_Controller.skinWidth + groundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        isGrounded = false;
        groundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(m_Controller.height), m_Controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    isGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > m_Controller.skinWidth)
                    {
                        m_Controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
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
