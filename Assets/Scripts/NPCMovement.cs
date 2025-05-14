using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints;
    public string playerTag = "Player";

    private NavMeshAgent agent;
    private NavMeshAgentAnimationSync animationSync;
    private DialogueRunner dialogueRunner;
    private Transform player;

    private int currentWaypoint = 0;
    private bool isDialoguePlaying = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animationSync = GetComponent<NavMeshAgentAnimationSync>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
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
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                StartCoroutine(StartDialogueRoutine());
            }
        }
    }

    void MoveToWaypoint()
    {
        animationSync.CurrentDestinaton = waypoints[currentWaypoint].position;
        agent.isStopped = false;
    }

    IEnumerator StartDialogueRoutine()
    {
        isDialoguePlaying = true;

        // Stop agent movement
        animationSync.StopMoving();

        // Face player
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // Start dialogue
        string nodeName = waypoints[currentWaypoint].name;
        if (!dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(nodeName);
        }

        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        // Continue to next waypoint
        currentWaypoint++;
        isDialoguePlaying = false;

        if (currentWaypoint < waypoints.Count)
        {
            MoveToWaypoint();
        }
    }
}
