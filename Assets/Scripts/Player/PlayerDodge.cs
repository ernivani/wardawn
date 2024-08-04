using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]
    public float dodgeCooldown = 1.0f;
    
    private PlayerMovementController movementController;
    private PlayerStamina staminaController;
    private PlayerColorController colorController;
    private bool canDodge;
    
    private void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        staminaController = GetComponent<PlayerStamina>();
        colorController = GetComponent<PlayerColorController>();

        canDodge = true;
    }

    public IEnumerator Dodge()
    {
        if (canDodge && staminaController.CanDodge())
        {
            Debug.Log("Player dodged.");
            PlayerInput.isDodging = true;
            canDodge = false;

            staminaController.DeductStamina(staminaController.dodgeStaminaCost);
            colorController.ChangeColor(Color.red);

            yield return new WaitForSeconds(movementController.dodgeDuration);

            PlayerInput.isDodging = false;
            if (PlayerInput.isSprinting)
            {
                colorController.ChangeColor(Color.green);
            }
            else
            {
                colorController.ResetColor();
            }

            yield return new WaitForSeconds(dodgeCooldown);
            canDodge = true;
        }
    }
}
