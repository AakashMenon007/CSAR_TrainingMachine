using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtPlayer : MonoBehaviour
{
    public Transform playerCamera; // Assign the player's camera in Inspector

    void LateUpdate()
    {
        if (playerCamera == null) return;

        Vector3 direction = transform.position - playerCamera.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
