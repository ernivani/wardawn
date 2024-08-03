using UnityEngine;

public class WardawnController : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5.0f;
    public float minDistance = 3.0f;
    public float maxDistance = 5.0f;
    public float smoothTime = 0.3f;
    public float playerSpeedFactor = 2.0f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (player != null)
        {
            AdjustPosition();
        }
    }

    private void AdjustPosition()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = transform.position;

        if (distanceToPlayer < minDistance)
        {
            targetPosition = player.position - directionToPlayer * minDistance;
        }
        else if (distanceToPlayer > maxDistance)
        {
            targetPosition = player.position + directionToPlayer * maxDistance;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        FacePlayer();
    }

    private void FacePlayer()
    {
        Vector3 directionToFace = player.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}
