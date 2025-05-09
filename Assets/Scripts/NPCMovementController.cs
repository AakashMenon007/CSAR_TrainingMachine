//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using Yarn.Unity;

//public class NPCMovementController : MonoBehaviour
//{
//    public List<Transform> componentTargets;
//    private int currentIndex = 0;
//    private NavMeshAgent agent;
//    private Animator animator;
//    private DialogueRunner dialogueRunner;
//    private Transform player;

//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();
//        animator = GetComponent<Animator>();
//        dialogueRunner = FindObjectOfType<DialogueRunner>();
//        player = GameObject.FindGameObjectWithTag("Player").transform;

//        // Initialize animation
//        animator.SetBool("move", false);
//        animator.SetFloat("locomotion", 0f);

//        if (componentTargets.Count > 0)
//        {
//            MoveToNextTarget();
//        }
//    }

//    void Update()
//    {
//        if (agent.pathPending || componentTargets.Count == 0 || currentIndex >= componentTargets.Count)
//            return;

//        // Check if reached destination
//        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance)
//        {
//            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
//            {
//                StartCoroutine(HandleDialogue());
//            }
//        }
//        else
//        {
//            // Update movement animation
//            animator.SetBool("move", true);
//            animator.SetFloat("locomotion", agent.velocity.magnitude);
//        }
//    }

//    void MoveToNextTarget()
//    {
//        if (currentIndex >= componentTargets.Count) return;

//        agent.SetDestination(componentTargets[currentIndex].position);
//        animator.SetBool("move", true);
//    }

//    IEnumerator HandleDialogue()
//    {
//        // Stop movement and animations
//        agent.isStopped = true;
//        animator.SetBool("move", false);
//        animator.SetFloat("locomotion", 0f);

//        // Face the player
//        Vector3 lookDirection = player.position - transform.position;
//        lookDirection.y = 0;
//        if (lookDirection != Vector3.zero)
//        {
//            transform.rotation = Quaternion.LookRotation(lookDirection);
//        }

//        // Start dialogue
//        string nodeName = componentTargets[currentIndex].name;
//        dialogueRunner.StartDialogue(nodeName);

//        // Wait for dialogue to complete
//        while (dialogueRunner.IsDialogueRunning)
//        {
//            yield return null;
//        }

//        // Prepare for next movement
//        currentIndex++;
//        agent.isStopped = false;
//        MoveToNextTarget();
//    }
//}