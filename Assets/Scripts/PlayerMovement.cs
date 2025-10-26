using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [SerializeField] float movementMultiplier = 10f;

    [SerializeField] public float movementSpeed;
    [SerializeField] float maxSpeed = 10f;

    [Header("Input (New Input System)")]
    [Tooltip("Reference a Vector2 Move action (e.g., WASD/Left Stick)")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Orientation")]
    [Tooltip("Transform used to determine movement direction (e.g., Camera or Player Model)")]
    public Transform orientation;

    public float groundDrag;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.2f;
    bool grounded;
    public float playerHeight;

    [Header("Jumping Variables")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.2f;
    [SerializeField] private float airMultiplier = 0.4f;
    private bool readyToJump;

    float horizontalInput;
    float verticalInput;
    Vector2 inputVector;



    Vector3 movementDirection;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Fallback if orientation is not assigned
        if (orientation == null)
            orientation = transform;

        // Allow jumping right away
        readyToJump = true;

        // Auto-detect player height if not set
        if (playerHeight <= 0f)
        {
            if (TryGetComponent<CapsuleCollider>(out var capsule))
                playerHeight = capsule.height;
            else if (TryGetComponent<Collider>(out var col))
                playerHeight = col.bounds.size.y;
        }
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (jumpAction != null) jumpAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (jumpAction != null) jumpAction.action.Disable();
    }

    private void ReadInput()
    {
        // Read player input using the New Input System
        if (moveAction == null)
        {
            inputVector = Vector2.zero;
            horizontalInput = 0f;
            verticalInput = 0f;
            return;
        }

        inputVector = moveAction.action.ReadValue<Vector2>();
        horizontalInput = inputVector.x;
        verticalInput = inputVector.y;

        if (jumpAction != null && jumpAction.action.enabled && jumpAction.action.triggered && readyToJump && grounded)
        {
            Jumping();
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction relative to orientation on XZ plane
        Vector3 forward = orientation != null ? orientation.forward : transform.forward;
        Vector3 right = orientation != null ? orientation.right : transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        movementDirection = forward * verticalInput + right * horizontalInput;
        movementDirection.Normalize();

        // Move the player
        if (grounded)
            rb.AddForce(movementDirection * movementSpeed * movementMultiplier, ForceMode.Force);
        else
            rb.AddForce(movementDirection * movementSpeed * movementMultiplier * airMultiplier, ForceMode.Force);

    }

    private void LimitHorizontalSpeed()
    {
        // Clamp horizontal (XZ) velocity to maxSpeed
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limited = flatVel.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }
    }

    private void Jumping()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            // Reset vertical velocity before jumping
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Invoke ResetJump after jumpCooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + groundCheckDistance, groundMask);
        ReadInput();

        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    void FixedUpdate()
    {
        MovePlayer();
        LimitHorizontalSpeed();
    }


}
