using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    public float maxHealth = 100f;

    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public abstract void TakeDamage(float amount);

    public virtual void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}
