using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    PlayerMovementController movementController;
    PlayerStamina staminaController;
    PlayerColorController colorController;
    PlayerDodge dodgeController;

    public static bool isSprinting { get; private set; }
    public static bool isDodging { get; set; }

    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        staminaController = GetComponent<PlayerStamina>();
        colorController = GetComponent<PlayerColorController>();
        dodgeController = GetComponent<PlayerDodge>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        HandleSprintInput();
        HandleJumpInput();
        HandleDodgeInput();
    }

    private void HandleSprintInput()
    {
        bool wasSprinting = isSprinting;
        isSprinting = Input.GetKey(sprintKey) && staminaController.CanSprint() && movementController.IsGrounded() && IsMoving();

        if (!isDodging)
        {
            if (isSprinting && !wasSprinting)
            {
                Debug.Log("Player started sprinting.");
                colorController.ChangeColor(Color.green);
            }
            else if (!isSprinting && wasSprinting)
            {
                Debug.Log("Player stopped sprinting.");
                colorController.ResetColor();
            }
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && movementController.IsGrounded())
        {
            movementController.Jump();
        }
    }

    private void HandleDodgeInput()
    {
        if (Input.GetKeyDown(sprintKey) && staminaController.CanDodge() && movementController.IsGrounded() && !isDodging && IsMoving())
        {
            StartCoroutine(dodgeController.Dodge());
        }
    }

    private bool IsMoving()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        return horizontalInput != 0 || verticalInput != 0;
    }
}
