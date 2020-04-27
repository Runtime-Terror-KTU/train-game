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
    public bool hasJumpedThisFrame { get; private set; }
    bool isJumping;
    Vector3 jumpDirection;
    Vector3 groundNormal;
    Vector3 characterVelocity;

    LayerMask groundCheckLayers = -1;

    public Animator animator;
    public float movSpeed = 6f;
    public float sideSpeed = 5f;
    public float backSpeed = 3f;
    float LastTimeJumped = 0f;

    public bool isFiring;
    public bool isReloading;

    public float gravityForce = 20f;
    public float maxSpeedGrounded = 10f;
    public float maxSpeedAir = 10f;
    public float movementAcceleration = 15;
    public float airAcceleration = 20f;
    public float jumpForce = 5f;
    const float JumpPreventionTime = 0.2f;
    public float GroundCheckDist = 0.07f;
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
        hasJumpedThisFrame = false;

        //bool wasGrounded = isGrounded;
        GroundCheck();

        //if (isGrounded && !wasGrounded)
        //    // fall damage?
        //    // land audio

        Movement();

    }

    void FixedUpdate()
    {
    }

    void Movement()
    {
        //if (walking)
        //    movSpeed = 3f;
        //    sideSpeed = 3f;
        //    backSpeed = 3f;
        //else
        //    movSpeed = 6;
        //    sideSpeed = 5f;
        //    backSpeed = 3f;

        //Vector3 inputsNormalized = new Vector3(inputs.x, inputs.y, inputs.z);
        Vector3 inputsNormalized = new Vector3(inputs.x, 0f, inputs.z);
        inputsNormalized.Normalize();

        Vector3 worldMoveInput = transform.TransformVector(inputsNormalized);

        //ground movement
        if (isGrounded)
        {
            Vector3 targetVelocity = worldMoveInput * maxSpeedGrounded;

            //if crouched?
            //smooth interpolation
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementAcceleration * Time.deltaTime);

            //jumping
            if (isGrounded && Input.GetKeyDown(controls.jump))
            {
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                characterVelocity += Vector3.up * jumpForce;

                //sound for jump

                //track time
                LastTimeJumped = Time.time;
                hasJumpedThisFrame = true;

                isGrounded = false;
                groundNormal = Vector3.up;
            }
            //footstep frequency, distance & sound
        }
        //air movement
        else
        {
            characterVelocity += worldMoveInput * airAcceleration * Time.deltaTime;

            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedAir);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            //gravity
            characterVelocity += Vector3.down * gravityForce * Time.deltaTime;
        }

        //apply velocity
        Vector3 capsuleBotBeforeMove = GetCapsuleBot();
        Vector3 capsuleTopBeforeMove = GetCapsuleTop(controller.height);
        controller.Move(characterVelocity * Time.deltaTime);

        if (Physics.CapsuleCast(capsuleBotBeforeMove, capsuleTopBeforeMove, controller.radius, characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
            characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
    }

    void GroundCheck()
    {
        isGrounded = false;
        groundNormal = Vector3.up;

        if (Time.time >= LastTimeJumped + JumpPreventionTime)
            if(Physics.CapsuleCast(GetCapsuleBot(), GetCapsuleTop(controller.height), controller.radius, Vector3.down, out RaycastHit hit, GroundCheckDist, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                groundNormal = hit.normal;

                if(Vector3.Dot(hit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(groundNormal))
                {
                    isGrounded = true;
                    if(hit.distance > controller.skinWidth)
                        controller.Move(Vector3.down * hit.distance);
                }
            }
    }

    Vector3 GetCapsuleBot()
    {
        return transform.position + (transform.up * controller.radius);
    }

    Vector3 GetCapsuleTop(float atH)
    {
        return transform.position + (transform.up * (atH - controller.radius));
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
    }

    void GetInputs()
    {
        //if (Input.GetKey(controls.jump))
        //    inputs.y = 1;
        //else inputs.y = 0;


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
