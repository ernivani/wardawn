using UnityEngine;

public class MenirInteract : Interactable
{
    [Header("Wardawn Settings")]
    public GameObject wardawnPrefab;
    public Transform playerGroup;
    public float spawnDistance = 3.0f;

    public override void Interact()
    {
        if (playerGroup == null || wardawnPrefab == null)
        {
            Debug.Log("PlayerGroup or WardawnPrefab not assigned.");
            return;
        }

        if (playerGroup.Find("Wardawn") != null)
        {
            Debug.Log("Wardawn already exists in PlayerGroup. No new instance created.");
            return;
        }

        Transform player = playerGroup.GetChild(0);
        if (player == null)
        {
            Debug.LogError("Player transform not found in PlayerGroup.");
            return;
        }

        Vector3 spawnPositon = CalculateSpawnPosition(player);

        GameObject wardawnInstance = Instantiate(wardawnPrefab, spawnPositon, Quaternion.identity, playerGroup);
        wardawnInstance.name = "Wardawn";

        WardawnController wardawnController = wardawnInstance.GetComponent<WardawnController>();
        if (wardawnController != null)
        {
            wardawnController.player = player;
            wardawnController.minDistance = 3.0f;
            wardawnController.maxDistance = 5.0f;
        }
        else
        {
            Debug.LogError("WardawnPrefab does not have a WardawnController component.");
        }

        Debug.Log("Wardawn spawned at a safe distance and will follow the player.");
    }

    private Vector3 CalculateSpawnPosition(Transform player)
    {
        Vector3 randomDirection = new Vector3(Random.value, 0, Random.value).normalized;
        Vector3 spawnPosition = player.position + randomDirection * spawnDistance;
        return spawnPosition;
    }
}
