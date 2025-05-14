using UnityEngine.AI;
using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavMeshAgentAnimationSync : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    [SerializeField]
    private LookAt _lookAt; // optional component to move head to look at target direction

    private Vector2 _velocity;
    private Vector2 _smoothDeltaPosition;

    private Vector3 _currentDestination;
    public Vector3 CurrentDestinaton
    {
        get { return _navMeshAgent.destination; }
        set { _navMeshAgent.SetDestination(value); }
    }
    public enum BlendTreeType
    {
        OneD,
        TwoD,
    }
    public BlendTreeType blendTreeType;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _lookAt = GetComponent<LookAt>();
        _animator.applyRootMotion = true;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true;
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = _animator.rootPosition;
        rootPosition.y = _navMeshAgent.nextPosition.y;
        transform.position = rootPosition;
        _navMeshAgent.nextPosition = rootPosition;
    }

    private void Update()
    {
        SynchronizeAnimatorAndAgent();
    }

    private void SynchronizeAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

        _velocity = _smoothDeltaPosition / Time.deltaTime;
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _velocity = Vector2.Lerp(Vector2.zero, _velocity, _navMeshAgent.remainingDistance);
        }

        bool shouldMove = _velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;

        _animator.SetBool("move", shouldMove);

        if (blendTreeType == BlendTreeType.OneD)
            _animator.SetFloat("locomotion", _velocity.magnitude);
        else if (blendTreeType == BlendTreeType.TwoD)
        {
            _animator.SetFloat("velx", _velocity.x);
            _animator.SetFloat("vely", _velocity.y);
        }

        if (_lookAt != null)
            _lookAt.lookAtTargetPosition = _navMeshAgent.steeringTarget + transform.forward;

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > _navMeshAgent.radius / 2)
        {
            transform.position = Vector3.Lerp(_animator.rootPosition, _navMeshAgent.nextPosition, smooth);
        }
    }

    public void StopMoving()
    {
        _navMeshAgent.isStopped = true;
    }
}