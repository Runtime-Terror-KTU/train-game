using UnityEngine;
using UnityEngine.Events;

public class PlayerControls : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Main camera used by the player")]
    public Camera playerCamera;

    [Header("General")]
    [Tooltip("Force applied down when in the air")]
    public float gravityDownForce = 20f;
    [Tooltip("Physic layers to check if player is grounded")]
    public LayerMask groundCheckLayers = -1;
    [Tooltip("Distance from the bottom of the character controller to test if grounded")]
    public float groundCheckDist = 0.05f;

    [Header("Movement")]
    [Tooltip("Max movement speed when grounded")]
    public float maxSpeedGrounded = 10f;
    [Tooltip("Ground movement acceleration speed")]
    public float movementAcceleration = 15f;
    [Tooltip("Max movement speed when not grounded")]
    public float maxSpeedAir = 15f;
    [Tooltip("Air movement acceleration speed")]
    public float airAcceleration = 25f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float rotationSpeed = 200f;

    [Header("Jump")]
    [Tooltip("Force applied upward when jumping")]
    public float jumpForce = 9f;

    [Header("Stance")]
    [Range(0.1f, 1f)]
    [Tooltip("Ratio (0-1) of the character height where the camera will be at")]
    public float cameraHeightRatio = 0.9f;


    public Vector3 characterVelocity { get; set; }
    public bool isGrounded { get; private set; }
    public bool hasJumpedThisFrame { get; private set; }
    Vector3 groundNormal;
    public float rotationMultiplier = 1f;


    CharacterController controller;
    InputHandler inputHandler;
    Player player;
    float lastTimeJumped = 0f;
    float cameraVerticalAngle = 0f;
    float targetCharacterHeight = 2.5f;

    const float jumpPreventionTime = 0.2f;
    const float groundCheckDistAir = 0.07f;

    //TEMPORARY
    public bool isFiring = false;
    public bool isReloading = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        player = GetComponent<Player>();

        controller.enableOverlapRecovery = true;
        UpdateCharacterHeight();
    }

    void Update()
    {
        if(PauseMenu.OnPause)
        {

        }
        else
        {
            hasJumpedThisFrame = false;

            //bool wasGrounded = isGrounded;
            GroundCheck();

            //if (isGrounded && !wasGrounded)
            //    // fall damage?
            //    // land audio
            UpdateCharacterHeight();
            Movement();
        }
    }
    void GroundCheck()
    {
        float chosenGroundCheckDistance = isGrounded ? (controller.skinWidth + groundCheckDist) : groundCheckDistAir;

        isGrounded = false;
        groundNormal = Vector3.up;

        if (Time.time >= lastTimeJumped + jumpPreventionTime)
        {
            if (Physics.CapsuleCast(GetCapsuleBot(), GetCapsuleTop(controller.height), controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                groundNormal = hit.normal;

                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(groundNormal))
                {
                    isGrounded = true;

                    if (hit.distance > controller.skinWidth)
                    {
                        controller.Move(Vector3.down * hit.distance);
                    }
                }
            }
        }
    }

    void Movement()
    {
        // horizontal rotation
        {
            transform.Rotate(new Vector3(0f, (inputHandler.GetLookInputsHorizontal() * rotationSpeed * rotationMultiplier), 0f), Space.Self);
        }

        // vertical camera rotation
        {
            cameraVerticalAngle += inputHandler.GetLookInputsVertical() * rotationSpeed * rotationMultiplier;
            cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);
            playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
        }

        float speedModifier = 1f;
        Vector3 worldMoveInput = transform.TransformVector(inputHandler.GetMoveInput());

        //ground movement
        if (isGrounded)
        {
            // calculate the desired velocity from inputs, max speed, and current slope
            Vector3 targetVelocity = worldMoveInput * maxSpeedGrounded * speedModifier;
            // reduce speed if crouching by crouch speed ratio
            targetVelocity = GetDirectionOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            characterVelocity = Vector3.Lerp(characterVelocity, targetVelocity, movementAcceleration * Time.deltaTime);

            // jumping
            if (isGrounded && inputHandler.GetJumpInputDown())
            {
                characterVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);
                characterVelocity += Vector3.up * jumpForce;

                // play sound

                lastTimeJumped = Time.time;
                hasJumpedThisFrame = true;
                isGrounded = false;
                groundNormal = Vector3.up;
            }
        }
        //air movement
        else
        {
            characterVelocity += worldMoveInput * airAcceleration * Time.deltaTime;

            float verticalVelocity = characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(characterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedAir * speedModifier);
            characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            // apply gravity
            characterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }

        // apply calculations
        Vector3 capsuleBottomBeforeMove = GetCapsuleBot();
        Vector3 capsuleTopBeforeMove = GetCapsuleTop(controller.height);
        controller.Move(characterVelocity * Time.deltaTime);

        // detect obstructions
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius, characterVelocity.normalized, out RaycastHit hit, characterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
            characterVelocity = Vector3.ProjectOnPlane(characterVelocity, hit.normal);
    }

    Vector3 GetCapsuleBot()
    {
        return transform.position + (transform.up * controller.radius);
    }

    Vector3 GetCapsuleTop(float atHeight)
    {
        return transform.position + (transform.up * (atHeight - controller.radius));
    }

    Vector3 GetDirectionOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }

    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
    }

    void UpdateCharacterHeight()
    {
        controller.height = targetCharacterHeight;
        controller.center = Vector3.up * controller.height * 0.5f;
        playerCamera.transform.localPosition = Vector3.up * targetCharacterHeight * cameraHeightRatio;
        //ACTOR.aimPoint.transform.localPosition = m_Controller.center;
    }
}
