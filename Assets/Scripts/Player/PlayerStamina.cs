using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaRegenRate = 5f;
    public float sprintStaminaCost = 1f;
    public float dodgeStaminaCost = 10f;

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
        if (PlayerInput.isSprinting && currentStamina > 0)
        {
            currentStamina -= sprintStaminaCost * Time.deltaTime;
        }
        else if (!PlayerInput.isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

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
