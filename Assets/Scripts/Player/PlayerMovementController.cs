using UnityEngine;

public class PlayerMovementController : Entity
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintMultiplier = 1.5f;
    public float dodgeMultiplier = 3.0f;
    public float dodgeDuration = 0.2f;
    public float groundDrag;
    public float airDrag = 2.0f;
    public float jumpForce;
    public float jumpCooldown;
    public float airControlMultiplier = 0.5f;
    public float additionalGravity = 5.0f;

    [Header("Ground Check")]
    public MeshRenderer playerRenderer;
    public LayerMask whatIsGround;

    public Transform orientation;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;
    [HideInInspector] public float dodgeSpeed;

    private Rigidbody rb;
    private bool readyToJump;
    private bool grounded;
    private Vector3 moveDirection;

    protected override void Start()
    {
        base.Start();
        
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        walkSpeed = moveSpeed;
        sprintSpeed = moveSpeed * sprintMultiplier;
        dodgeSpeed = moveSpeed * dodgeMultiplier;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerRenderer.bounds.extents.y + 0.3f, whatIsGround);

        rb.drag = grounded ? groundDrag : airDrag;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        MovePlayer();
    }

    private void ApplyGravity()
    {
        if (!grounded)
        {
            rb.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
        }
    }

    public void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = PlayerInput.isDodging ? dodgeSpeed : (PlayerInput.isSprinting ? sprintSpeed : walkSpeed);

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            // Apply less force in the air for control
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airControlMultiplier, ForceMode.Force);
        }
    }

    public void Jump()
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            // Reset vertical velocity before jumping
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float maxSpeed = PlayerInput.isDodging ? dodgeSpeed : (PlayerInput.isSprinting ? sprintSpeed : walkSpeed);

        // Clamp horizontal velocity
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        // Optional: Clamp vertical velocity to prevent excessive speed on falls
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeed, jumpForce), rb.velocity.z);
    }

    public bool IsGrounded()
    {
        return grounded;
    }
    public override void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (!IsAlive())
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}
