using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPCMovementController : MonoBehaviour
{
    public List<Transform> targetPoints;
    private NavMeshAgent agent;
    private DialogueRunner dialogueRunner;
    private Animator animator;  

    private int currentTargetIndex = 0;
    private bool isMoving = false;
    private bool isDialogueRunning = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        animator = GetComponent<Animator>(); 
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

        // Update animation based on movement
        if (animator != null)
        {
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }
    }

    void MoveToNextTarget()
    {
        if (currentTargetIndex < targetPoints.Count)
        {
            agent.SetDestination(targetPoints[currentTargetIndex].position);
            isMoving = true;
            animator.SetBool("isTalking", false); // Stop talking
        }
        else
        {
            ShowFinalRepeatMenu();
        }
    }

    IEnumerator TriggerDialogueAtCurrentTarget()
    {
        isMoving = false;
        isDialogueRunning = true;

        // Stop walking, start talking
        animator.SetBool("isWalking", false);
        animator.SetBool("isTalking", true);

        string nodeName = targetPoints[currentTargetIndex].name;
        dialogueRunner.StartDialogue(nodeName);

        while (dialogueRunner.IsDialogueRunning)
            yield return null;

        currentTargetIndex++;
        isDialogueRunning = false;
        MoveToNextTarget();
    }

    void ShowFinalRepeatMenu()
    {
        Debug.Log("All components visited.");
        animator.SetBool("isTalking", false);
        // Show repeat UI (we'll make it follow NPC in Part 2)
    }
}