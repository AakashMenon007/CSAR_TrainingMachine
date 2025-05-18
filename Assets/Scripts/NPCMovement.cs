using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    [Header("Waypoints (in order)")]
    public List<Transform> waypoints;

    [Header("Highlightable Objects (must match waypoint count)")]
    public List<GameObject> highlightObjects;

    [Header("Player Targeting")]
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
        if (currentWaypoint < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[currentWaypoint].position;
            animationSync.CurrentDestinaton = targetPosition;
            agent.SetDestination(targetPosition);
            agent.isStopped = false;
        }
    }

    IEnumerator StartDialogueRoutine()
    {
        isDialoguePlaying = true;
        animationSync.StopMoving();

        // Face the player
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // Highlight associated object
        if (currentWaypoint < highlightObjects.Count)
        {
            var highlighter = highlightObjects[currentWaypoint].GetComponent<MultiRendererHighlighter>();
            highlighter?.Highlight();
        }

        // Start Yarn dialogue
        string nodeName = waypoints[currentWaypoint].name;
        if (!dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(nodeName);
        }

        // Wait for dialogue to finish
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        // Remove highlight
        if (currentWaypoint < highlightObjects.Count)
        {
            var highlighter = highlightObjects[currentWaypoint].GetComponent<MultiRendererHighlighter>();
            highlighter?.StopHighlight();
        }

        // Advance
        currentWaypoint++;
        isDialoguePlaying = false;

        if (currentWaypoint < waypoints.Count)
        {
            MoveToWaypoint();
        }
    }
}
