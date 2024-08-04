using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 5f;
    public float sprintStaminaCost = 1f;
    public float dodgeStaminaCost = 10f;
    public float regenDelay = 1.0f;
    private float regenTimer;

    public Image staminaBar;

    private float currentStamina;
    private bool canSprint;

    private PlayerMovementController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerMovementController>();

        currentStamina = maxStamina;
        canSprint = true;
    }

    private void Update()
    {
        HandleStamina();
    }

    private void HandleStamina()
    {
        if (PlayerInput.isSprinting)
        {
            currentStamina -= sprintStaminaCost * Time.deltaTime;
            regenTimer = regenDelay;
        }
        else
        {
            if (regenTimer > 0)
            {
                regenTimer -= Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
            }
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        canSprint = currentStamina > 0;

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
        }
    }

    public bool CanSprint()
    {
        return canSprint;
    }

    public bool CanDodge()
    {
        return currentStamina >= dodgeStaminaCost;
    }

    public void DeductStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}
