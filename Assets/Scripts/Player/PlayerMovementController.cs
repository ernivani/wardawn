using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintMultiplier = 1.5f;  // Sprint speed multiplier
    public float dodgeMultiplier = 3.0f;   // Dodge speed multiplier
    public float dodgeDuration = 0.2f;     // Duration of the dodge
    public float dodgeCooldown = 1.0f;     // Cooldown between dodges
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool canDodge = true; // Indicates if the player can dodge

    [Header("Stamina")]
    public float maxStamina = 100f;   // Maximum stamina
    public float currentStamina;      // Current stamina
    public float staminaRegenRate = 5f;  // Stamina regeneration rate per second
    public float sprintStaminaCost = 1f; // Stamina cost per second when sprinting
    public float dodgeStaminaCost = 10f; // Stamina cost for a dodge
    bool canSprint = true;  // Indicates if the player can sprint

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;
    [HideInInspector] public float dodgeSpeed;

    public Image staminaBar;  // Reference to the stamina bar UI

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;  // Sprint/Dodge key

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    bool isSprinting;
    bool isDodging;

    Vector3 moveDirection;

    Rigidbody rb;
    public MeshRenderer playerRenderer;
    Color originalColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        walkSpeed = moveSpeed;  // Set the base walk speed
        sprintSpeed = moveSpeed * sprintMultiplier;  // Calculate sprint speed
        dodgeSpeed = moveSpeed * dodgeMultiplier;  // Calculate dodge speed

        // Get the player's Renderer component and original color
        originalColor = playerRenderer.material.color;

        // Initialize stamina
        currentStamina = maxStamina;
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StaminaControl();  // Handle stamina

        // Handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check for sprint input
        bool wasSprinting = isSprinting;
        isSprinting = Input.GetKey(sprintKey) && canSprint && IsMoving();

        // Handle sprint color change
        if (!isDodging) // Only change color if not dodging
        {
            if (isSprinting && !wasSprinting)
            {
                Debug.Log("Player started sprinting.");
                ChangeColor(Color.green); // Change color to green when sprinting starts
            }
            else if (!isSprinting && wasSprinting)
            {
                Debug.Log("Player stopped sprinting.");
                ResetColor(); // Reset color when sprinting stops
            }
        }

        // Handle jump input
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Handle dodge input
        if (Input.GetKeyDown(sprintKey) && canDodge && grounded && !isDodging && currentStamina >= dodgeStaminaCost && IsMoving()) // Prevent dodge while already dodging and check stamina
        {
            StartCoroutine(Dodge());
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Determine current speed based on whether the player is sprinting or dodging
        float currentSpeed = isDodging ? dodgeSpeed : (isSprinting ? sprintSpeed : walkSpeed);

        // On ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);

        // In air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * currentSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Limit velocity if needed
        float maxSpeed = isDodging ? dodgeSpeed : (isSprinting ? sprintSpeed : walkSpeed);
        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private IEnumerator Dodge()
    {
        Debug.Log("Player dodged."); // Log when the player dodges

        isDodging = true;
        canDodge = false;
        currentStamina -= dodgeStaminaCost; // Deduct stamina for dodging
        ChangeColor(Color.red); // Change color to red when dodging

        yield return new WaitForSeconds(dodgeDuration);

        isDodging = false;
        if (isSprinting) // Check if the player is sprinting after dodge
        {
            ChangeColor(Color.green); // Change to green if sprinting
        }
        else
        {
            ResetColor(); // Reset color after dodging if not sprinting
        }

        yield return new WaitForSeconds(dodgeCooldown);

        canDodge = true;
    }

    private void StaminaControl()
    {
        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= sprintStaminaCost * Time.deltaTime; // Reduce stamina when sprinting
        }
        else if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime; // Regenerate stamina when not sprinting
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Ensure stamina is within bounds

        // Disable sprinting if stamina is depleted
        if (currentStamina <= 0)
        {
            canSprint = false;
        }
        else
        {
            canSprint = true;
        }

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina; // Update the stamina bar UI
        }
    }

    private void ChangeColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }

    private void ResetColor()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }

    // Helper function to check if player is moving
    private bool IsMoving()
    {
        return horizontalInput != 0 || verticalInput != 0;
    }
}
