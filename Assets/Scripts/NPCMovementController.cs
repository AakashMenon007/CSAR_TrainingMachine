using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPCMovementController : MonoBehaviour
{
    public List<Transform> targetPoints; // Assign cubes in order
    private NavMeshAgent agent;
    private DialogueRunner dialogueRunner;
    private int currentTargetIndex = 0;
    private bool isMoving = false;
    private bool isDialogueRunning = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        MoveToNextTarget();
    }

    void Update()
    {
        if (isMoving && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!isDialogueRunning)
            {
                StartCoroutine(TriggerDialogueAtCurrentTarget());
            }
        }
    }

    void MoveToNextTarget()
    {
        if (currentTargetIndex < targetPoints.Count)
        {
            agent.SetDestination(targetPoints[currentTargetIndex].position);
            isMoving = true;
        }
        else
        {
            ShowFinalRepeatMenu(); // Handle your canvas activation here
        }
    }

    IEnumerator TriggerDialogueAtCurrentTarget()
    {
        isMoving = false;
        isDialogueRunning = true;

        string nodeName = targetPoints[currentTargetIndex].name;
        dialogueRunner.StartDialogue(nodeName);

        // Wait until dialogue finishes
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        currentTargetIndex++;
        isDialogueRunning = false;
        MoveToNextTarget();
    }

    void ShowFinalRepeatMenu()
    {
        Debug.Log("All components visited. Show repeat menu.");
        // Your canvas logic here
    }
}
