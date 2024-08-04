using UnityEngine;

public class PlayerColorController : MonoBehaviour
{
    private Color originalColor;
    private Renderer playerRenderer;

    private void Start()
    {
        playerRenderer = GetComponent<PlayerMovementController>().playerRenderer;
        if (playerRenderer != null)
        {
            originalColor = playerRenderer.material.color;
        }
    }

    public void ChangeColor(Color color)
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = color;
        }
    }

    public void ResetColor()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.color = originalColor;
        }
    }
}
