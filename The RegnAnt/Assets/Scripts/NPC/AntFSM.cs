using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AntFSM : MonoBehaviour
{
    private FiniteStateMachine<AntFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;

    [Range(0, 100)] public float speed;
    [Range(0, 500)] public float walkRadius;

    Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = speed;

        _stateMachine = new FiniteStateMachine<AntFSM>(this);

        //STATES
        State wanderState = new WanderState("Wander", this);
        /*State followRailState = new followRailState("followRail", this);
        State loadObjectState = new loadObjectState("loadObject", this);
        State moveToDestinationState = new moveToDestinationState("moveToDestination", this);*/

        //TRANSITIONS
        /*_stateMachine.AddTransition(patrolState, chaseState, () => DistanceFromTarget() <= _minChaseDistance);
        _stateMachine.AddTransition(chaseState,patrolState, () => DistanceFromTarget() > _minChaseDistance);
        _stateMachine.AddTransition(chaseState,stopState, () => DistanceFromTarget() <= _stoppingDistance);
        _stateMachine.AddTransition(stopState,chaseState, () => DistanceFromTarget() > _stoppingDistance);*/

        //START STATE
        _stateMachine.SetState(wanderState);
    }

    // Update is called once per frame
    void Update() => _stateMachine.Tik();

    /*public void SetWayPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && _navMeshAgent.velocity.sqrMagnitude <= 0f)
        {
            _currentWayPointIndex = (_currentWayPointIndex + 1) % _waypoints.Count;
            Vector3 nextWayPointPos = _waypoints[_currentWayPointIndex].position;
            _navMeshAgent.SetDestination(new Vector3(nextWayPointPos.x, transform.position.y, nextWayPointPos.z));
        }
    }*/

    public void SetRandomPointDestination()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 2f) 
        {
            destination = RandomNavMeshLocation();
            _navMeshAgent.SetDestination(destination);
        }                  
            
        
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if(NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
            finalPosition = hit.position;

        return finalPosition;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.5f);
    }
}

public class WanderState : State
{
    private AntFSM _ant;
    public WanderState(string name, AntFSM ant) : base(name)
    {
        _ant = ant;
    }

    public override void Enter()
    {
        /*_guard.StopAgent(false);
        _guard.Renderer.material.color = _guard.OriginalColor;*/
    }

    public override void Tik()
    {
        Debug.Log("Tick");
        _ant.SetRandomPointDestination();
    }

    public override void Exit()
    {
        
    }
    
}