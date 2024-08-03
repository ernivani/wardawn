using System.Collections;
using System.Collections.Generic;
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
        // Check for sprint input
        bool wasSprinting = isSprinting;
        isSprinting = Input.GetKey(sprintKey) && staminaController.CanSprint() && movementController.IsGrounded() && IsMoving();

        // Handle sprint color change
        if (!isDodging) // Only change color if not dodging
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

        // Handle jump input
        if (Input.GetKey(jumpKey))
        {
            movementController.Jump();
        }

        // Handle dodge input
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
