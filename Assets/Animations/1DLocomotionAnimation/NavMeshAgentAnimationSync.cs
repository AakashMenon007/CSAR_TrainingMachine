//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using Yarn.Unity;

//public class NPCMovementController : MonoBehaviour
//{
//    public List<Transform> componentTargets; // cubes with component names
//    private int currentIndex = 0;
//    private NavMeshAgent agent;
//    private Animator animator;
//    private DialogueRunner dialogueRunner;
//    private NavMeshAgentAnimationSync animationSync;
//    private bool isTalking = false;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//        dialogueRunner = FindObjectOfType<DialogueRunner>();
//        animationSync = GetComponent<NavMeshAgentAnimationSync>();

//        // Ensure agent is properly configured for animation sync
//        agent.stoppingDistance = 0.1f;
//        agent.autoBraking = true;

//        MoveToNextTarget();
//    }

//    void Update()
//    {
//        if (isTalking || componentTargets.Count == 0 || currentIndex >= componentTargets.Count)
//            return;

//        // Check if reached destination and not already talking
//        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
//        {
//            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
//            {
//                StartCoroutine(HandleDialogue());
//            }
//        }
//    }

//    void MoveToNextTarget()
//    {
//        if (currentIndex >= componentTargets.Count)
//        {
//            // Finished all targets
//            return;
//        }

//        agent.isStopped = false;
//        agent.SetDestination(componentTargets[currentIndex].position);
//    }

//    IEnumerator HandleDialogue()
//    {
//        isTalking = true;

//        // Face the target while talking
//        Vector3 lookDirection = componentTargets[currentIndex].position - transform.position;
//        lookDirection.y = 0;
//        if (lookDirection != Vector3.zero)
//        {
//            transform.rotation = Quaternion.LookRotation(lookDirection);
//        }

//        // Trigger dialogue using object name as node
//        string nodeName = componentTargets[currentIndex].name;
//        dialogueRunner.StartDialogue(nodeName);

//        // Wait for Yarn to finish
//        while (dialogueRunner.IsDialogueRunning)
//        {
//            yield return null;
//        }

//        currentIndex++;
//        isTalking = false;

//        MoveToNextTarget();
//    }
//}