using UnityEngine;
using UnityEngine.AI;

public class AgentMoveToPlayer : Follow
{
    [SerializeField] private float _stoppingDistance = 2f;

    [SerializeField] private NavMeshAgent _agent;
    
    private Transform _heroTransform;

    public void Construct(Transform heroTransform) => 
        _heroTransform = heroTransform;

    private void Start() => 
        _agent.stoppingDistance = _stoppingDistance;

    private void Update()
    {
        if (Inited())
            _agent.destination = _heroTransform.position;
    }

    private bool Inited() =>
        _heroTransform != null;
}
