using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class spiderFSM : MonoBehaviour
{
    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius;

    [SerializeField] private float _minDistance;
    private FiniteStateMachine<spiderFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _terrain;
    Vector3 destination;
    bool test = false;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _stateMachine = new FiniteStateMachine<spiderFSM>(this);

        //STATES
        State runState = new RunState("Run", this);
        State attackState = new AttackState("Attack", this);
        State attackCloseState = new AttackState("Attack", this);

        //TRANSITIONS
        _stateMachine.AddTransition(runState, attackState, () => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);
        _stateMachine.AddTransition(attackState, attackCloseState, () => test);

        //START STATE
        _stateMachine.SetState(runState);
    }

    // Update is called once per frame
    void Update() => _stateMachine.Tik();

    public void SetRandomPointDestination()
    {
        
        destination = RandomNavMeshLocation();
        _navMeshAgent.SetDestination(destination);

    }

    private Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        NavMeshHit hit;
        Vector3 randomPosition;
        do
        {
            randomPosition = UnityEngine.Random.insideUnitSphere * walkRadius;
            randomPosition += transform.position;
        } while (!NavMesh.SamplePosition(randomPosition, out hit, walkRadius, 1) || Vector3.Distance(transform.position, hit.position) < _minDistance);

        finalPosition = hit.position;

        return finalPosition;
    }



    public void RotateTowards()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 15f)
        {
            Vector3 direction = (_terrain.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime *2f);
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 5f);
    }
}
