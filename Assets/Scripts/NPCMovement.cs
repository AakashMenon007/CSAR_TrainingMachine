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

    [Header("Highlightable Objects")]
    public List<GameObject> highlightObjects; // One per waypoint

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

        // Highlight the associated object
        if (currentWaypoint < highlightObjects.Count)
        {
            var highlighter = highlightObjects[currentWaypoint].GetComponent<Highlighter>();
            highlighter?.Highlight();
        }

        // Start Yarn dialogue
        string nodeName = waypoints[currentWaypoint].name;
        if (!dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue(nodeName);
        }

        // Wait until dialogue ends
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }

        // Remove highlight
        if (currentWaypoint < highlightObjects.Count)
        {
            var highlighter = highlightObjects[currentWaypoint].GetComponent<Highlighter>();
            highlighter?.RemoveHighlight();
        }

        // Proceed to next
        currentWaypoint++;
        isDialoguePlaying = false;

        if (currentWaypoint < waypoints.Count)
        {
            MoveToWaypoint();
        }
    }
}
