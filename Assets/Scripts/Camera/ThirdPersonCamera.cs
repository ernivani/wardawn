using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() 
    {
        // rotate orientation 
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // rotate playerObj
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDir = orientation.forward * vertical + orientation.right * horizontal;

        if (moveDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, moveDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }


}