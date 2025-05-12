using UnityEngine;

public class DynamicCanvasSpawner : MonoBehaviour
{
    public Canvas dialogCanvas;
    public Transform playerCamera;
    public float spawnDistance = 1.5f;

    void Start()
    {
        PositionCanvas();
    }

    void PositionCanvas()
    {
        // Position canvas in front of the player
        Vector3 spawnPos = playerCamera.position +
                          playerCamera.forward * spawnDistance;
        dialogCanvas.transform.position = spawnPos;

        // Align canvas to face the player
        dialogCanvas.transform.rotation = Quaternion.LookRotation(
            dialogCanvas.transform.position - playerCamera.position
        );
    }
}
