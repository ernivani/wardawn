using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    [Header("Camera Settings")]
    [Tooltip("Speed at which the camera rotates.")]
    public float rotationSpeed = 5.0f;
    [Tooltip("Speed at which the camera follows the player.")]
    public float followSpeed = 10.0f;
    [Tooltip("Offset from the player.")]
    public Vector3 offset = new Vector3(0, 2, -4);

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() 
    {
        HandleCameraMovement();
    }

    private void HandleCameraMovement()
    {
        // Calculate desired position based on offset
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Rotate orientation to face the player
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate playerObj based on input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDir = orientation.forward * vertical + orientation.right * horizontal;

        if(moveDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, moveDir.normalized, rotationSpeed * Time.deltaTime);
        }
    }
}
