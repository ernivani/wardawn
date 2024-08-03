using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenirInteract : Interactable
{
    public GameObject wardawnPrefab;
    public Transform playerGroup;
    public float spawnDistance = 3.0f;

    public override void Interact()
    {
        if (playerGroup == null || wardawnPrefab == null)
        {
            Debug.LogError("PlayerGroup or WardawnPrefab is not assigned.");
            return;
        }

        Transform existingWardawn = playerGroup.Find("Wardawn");

        if (existingWardawn == null)
        {
            Transform player = playerGroup.GetChild(0);
            Vector3 spawnPosition = CalculateSpawnPosition(player);
            GameObject wardawnInstance = Instantiate(wardawnPrefab, spawnPosition, Quaternion.identity, playerGroup);
            wardawnInstance.name = "Wardawn";

            WardawnController wardawnController = wardawnInstance.GetComponent<WardawnController>();
            if (wardawnController != null)
            {
                wardawnController.player = player;
                wardawnController.minDistance = 3.0f;
                wardawnController.maxDistance = 5.0f;
            }

            Debug.Log("Wardawn spawned at a safe distance and will follow the player.");
        }
        else
        {
            Debug.Log("Wardawn already exists in PlayerGroup. No new instance created.");
        }
    }

    private Vector3 CalculateSpawnPosition(Transform player)
    {
        Vector3 randomDirection = new Vector3(Random.value, 0, Random.value).normalized;
        Vector3 spawnPosition = player.position + randomDirection * spawnDistance;
        return spawnPosition;
    }
}
