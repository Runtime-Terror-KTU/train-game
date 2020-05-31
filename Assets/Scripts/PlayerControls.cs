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
    [Tooltip("Max movement speed when crouching")]
    [Range(0, 1)]
    public float maxSpeedCrouchedRatio = 0.5f;
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
    [Tooltip("Height of character when standing")]
    public float capsuleHeightStanding = 1.8f;
    [Tooltip("Height of character when crouching")]
    public float capsuleHeightCrouching = 0.9f;
    [Tooltip("Speed of crouching transitions")]
    public float crouchingSharpness = 10f;

    public UnityAction<bool> onStanceChanged;

    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }
    public bool HasJumpedThisFrame { get; private set; }
    public bool IsCrouching { get; private set; }
    public float rotationMultiplier = 1f;
    Vector3 groundNormal;


    CharacterController controller;
    InputHandler inputHandler;
    BoxCollider boxCollider;
    float lastTimeJumped = 0f;
    float cameraVerticalAngle = 0f;
    float targetCharacterHeight;

    const float jumpPreventionTime = 0.2f;
    const float groundCheckDistAir = 0.07f;

    //TEMPORARY
    public bool isFiring = false;
    public bool isReloading = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        boxCollider = GetComponent<BoxCollider>();

        controller.enableOverlapRecovery = true;
        SetCrouchingState(false, true);
        UpdateCharacterHeight(true);
    }

    void Update()
    {
        if(PauseMenu.OnPause)
        {

        }
        else
        {
            HasJumpedThisFrame = false;

            //bool wasGrounded = isGrounded;
            GroundCheck();

            //if (isGrounded && !wasGrounded)
            //    // fall damage?
            //    // land audio
            if (inputHandler.GetCrouchInputDown())
            {
                SetCrouchingState(!IsCrouching, false);
            }

            UpdateCharacterHeight(false);
            Movement();
            Shooting();
        }
    }

    void Shooting()
    {
        //check for shots
        if (inputHandler.GetFireInputDown() || inputHandler.GetFireInputHeld())
            isFiring = true;
        else
            isFiring = false;
        //check for reload
        if (inputHandler.GetReloadInputDown())
            isReloading = true;
        else
            isReloading = false;
    }

    void GroundCheck()
    {
        float chosenGroundCheckDistance = IsGrounded ? (controller.skinWidth + groundCheckDist) : groundCheckDistAir;

        IsGrounded = false;
        groundNormal = Vector3.up;

        if (Time.time >= lastTimeJumped + jumpPreventionTime)
        {
            if (Physics.CapsuleCast(GetCapsuleBot(), GetCapsuleTop(controller.height), controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, groundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                groundNormal = hit.normal;

                if (Vector3.Dot(hit.normal, transform.up) > 0f &&
                    IsNormalUnderSlopeLimit(groundNormal))
                {
                    IsGrounded = true;

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
        if (IsGrounded)
        {
            Vector3 targetVelocity = worldMoveInput * maxSpeedGrounded * speedModifier;

            if (IsCrouching)
                targetVelocity *= maxSpeedCrouchedRatio;
            targetVelocity = GetDirectionOnSlope(targetVelocity.normalized, groundNormal) * targetVelocity.magnitude;

            // velocity interpolation
            CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, movementAcceleration * Time.deltaTime);

            // jumping
            if (IsGrounded && inputHandler.GetJumpInputDown())
            {
                CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
                CharacterVelocity += Vector3.up * jumpForce;

                // play sound

                lastTimeJumped = Time.time;
                HasJumpedThisFrame = true;
                IsGrounded = false;
                groundNormal = Vector3.up;
            }
        }
        //air movement
        else
        {
            CharacterVelocity += worldMoveInput * airAcceleration * Time.deltaTime;

            float verticalVelocity = CharacterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(CharacterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeedAir * speedModifier);
            CharacterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            // apply gravity
            CharacterVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }

        // apply calculations
        Vector3 capsuleBottomBeforeMove = GetCapsuleBot();
        Vector3 capsuleTopBeforeMove = GetCapsuleTop(controller.height);
        controller.Move(CharacterVelocity * Time.deltaTime);

        // detect obstructions
        if (Physics.CapsuleCast(capsuleBottomBeforeMove, capsuleTopBeforeMove, controller.radius, CharacterVelocity.normalized, out RaycastHit hit, CharacterVelocity.magnitude * Time.deltaTime, -1, QueryTriggerInteraction.Ignore))
            CharacterVelocity = Vector3.ProjectOnPlane(CharacterVelocity, hit.normal);
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

    void UpdateCharacterHeight(bool force)
    {
        // Update height instantly
        if (force)
        {
            controller.height = targetCharacterHeight;
            controller.center = Vector3.up * controller.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * targetCharacterHeight * cameraHeightRatio;
            boxCollider.center = Vector3.up * controller.height * 0.5f;
            boxCollider.size = new Vector3(boxCollider.size.x, targetCharacterHeight, boxCollider.size.x);
            //aimPoint.transform.localPosition = controller.center;
        }
        // Update height smoothly
        else if (controller.height != targetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            controller.height = Mathf.Lerp(controller.height, targetCharacterHeight, crouchingSharpness * Time.deltaTime);
            controller.center = Vector3.up * controller.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * targetCharacterHeight * cameraHeightRatio, crouchingSharpness * Time.deltaTime);
            boxCollider.center = Vector3.up * controller.height * 0.5f;
            boxCollider.size = new Vector3(boxCollider.size.x, targetCharacterHeight, boxCollider.size.x);
            //aimPoint.transform.localPosition = m_Controller.center;
        }
    }

    bool SetCrouchingState(bool crouched, bool ignoreObstructions)
    {
        // set appropriate heights
        if (crouched)
        {
            targetCharacterHeight = capsuleHeightCrouching;
        }
        else
        {
            // Detect obstructions
            if (!ignoreObstructions)
            {
                Collider[] standingOverlaps = Physics.OverlapCapsule(GetCapsuleBot(), GetCapsuleTop(capsuleHeightStanding), controller.radius, -1, QueryTriggerInteraction.Ignore);
                foreach (Collider c in standingOverlaps)
                    if (c != controller)
                        return false;
            }
            targetCharacterHeight = capsuleHeightStanding;
        }

        if (onStanceChanged != null)
            onStanceChanged.Invoke(crouched);

        IsCrouching = crouched;
        return true;
    }
}
