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

    private float _squaredDetectionRadius;

    private void Start()
    {
        _squaredDetectionRadius = detectionRadius * detectionRadius;
    }

    private void Update()
    {
        bool playerInRange = IsPlayerInRange();

        text.text = playerInRange ? "Interact" : "";

        MyInput(playerInRange);
    }

    private void MyInput(bool playerInRange)
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        gameObject.GetComponent<Interactable>().Interact();
    }

    private bool IsPlayerInRange()
    {
        return (player.position - transform.position).sqrMagnitude < _squaredDetectionRadius;
    }
}
