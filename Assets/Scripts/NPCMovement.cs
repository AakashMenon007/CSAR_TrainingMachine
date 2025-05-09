using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NPCMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public List<Transform> waypoints;
    public string playerTag = "Player";

    [Header("Animation Parameters")]
    public string moveParam = "move";
    public string speedParam = "locomotion";

    private NavMeshAgent agent;
    private Animator animator;
    private DialogueRunner dialogueRunner;
    private Transform player;
    private int currentWaypoint = 0;
    private bool isDialoguePlaying = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
    }

    void Start()
    {
        if (waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    void Update()
    {
        if (currentWaypoint >= waypoints.Count || isDialoguePlaying) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(StartDialogueRoutine());
            }
        }
        else
        {
            UpdateMovementAnimation();
        }
    }

    void MoveToWaypoint()
    {
        agent.SetDestination(waypoints[currentWaypoint].position);
        animator.SetBool(moveParam, true);
    }

    void UpdateMovementAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat(speedParam, speed);
    }

    IEnumerator StartDialogueRoutine()
    {
        isDialoguePlaying = true;

        // Stop moving
        agent.isStopped = true;
        animator.SetBool(moveParam, false);
        animator.SetFloat(speedParam, 0f);

        // Face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Start dialogue
        dialogueRunner.StartDialogue(waypoints[currentWaypoint].name);

        // Wait until dialogue ends
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        // Go to next point
        currentWaypoint++;
        agent.isStopped = false;
        isDialoguePlaying = false;

        if (currentWaypoint < waypoints.Count)
        {
            MoveToWaypoint();
        }
    }
}
