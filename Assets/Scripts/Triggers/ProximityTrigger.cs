using UnityEngine;
using TMPro;

public class ProximityTrigger : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform player;

    [Header("Detection Settings")]
    public float detectionRadius = 5.0f;

    [Header("UI")]
    public TextMeshProUGUI text;

    [Header("Keybindings")]
    public KeyCode interactionKey = KeyCode.E;

    private float _squaredDetectionRadius;
    private bool _wasInRangeLastFrame;

    private void Start()
    {
        _squaredDetectionRadius = detectionRadius * detectionRadius;
    }

    private void Update()
    {
        bool playerInRange = IsPlayerInRange();

        if (playerInRange != _wasInRangeLastFrame)
        {
            text.text = playerInRange ? "Interact" : "";
            _wasInRangeLastFrame = playerInRange;
        }

        MyInput(playerInRange);
    }

    private void MyInput(bool playerInRange)
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Interactable interactable = gameObject.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
        else
        {
            Debug.LogError("Interactable component not found on the game object.");
        }
    }

    private bool IsPlayerInRange()
    {
        return (player.position - transform.position).sqrMagnitude < _squaredDetectionRadius;
    }
}
