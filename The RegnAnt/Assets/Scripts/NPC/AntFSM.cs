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

    private RaycastHit _hitGround;    
    public float offset_y;

    [SerializeField] private Transform _objectToLoad;
    bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = speed;

        _stateMachine = new FiniteStateMachine<AntFSM>(this);
        _objectToLoad = null;

        //STATES
        State wanderState = new WanderState("Wander", this);
        State loadObjectState = new LoadObjectState("loadObject", this);

        State followRailState = new FollowRailState("followRail", this);        
        //State moveObjectToDestinationState = new moveObjectToDestinationState("moveToDestination", this);

        //TRANSITIONS
        _stateMachine.AddTransition(wanderState, loadObjectState, () => _objectToLoad != null);
        _stateMachine.AddTransition(loadObjectState, followRailState, () => test == true);
        _stateMachine.AddTransition(wanderState, followRailState, () => test == true);
        /*_stateMachine.AddTransition(chaseState,stopState, () => DistanceFromTarget() <= _stoppingDistance);
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
            //wait for seconds?
            //yield return WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
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

    public void LookAround()
    {
        Debug.DrawLine(transform.position + new Vector3(0, offset_y, 0), transform.position + new Vector3(0, offset_y, 0) + 5f * transform.forward);
        if(Physics.SphereCast(transform.position + new Vector3(0, offset_y, 0), 1f, transform.forward, out RaycastHit hit, 5f)) 
        {   
            Debug.Log("Something Detected " + hit.transform.gameObject.name);
            //if food, layer, pheromones
            if(hit.transform.gameObject.layer == 7) //food layer
            {
                Debug.Log("Food detected");
                //move to object state
                _objectToLoad = hit.transform;
            }            
        } 
    }


    public void moveToFood()
    {
         _navMeshAgent.SetDestination(_objectToLoad.position - _navMeshAgent.stoppingDistance * transform.forward); 
    }

    /*public void groundCheck() 
    {     
        if(Physics.SphereCast(transform.position + new Vector3(0, offset_y, 0), 1f, -transform.up, out _hitGround, 1f)) 
        {   
            rotateToSurfaceNormal(_hitGround.normal, 1f);
        }                   
    }*/

    /*private void rotateToSurfaceNormal(Vector3 vectorToReach, float speed)
    {
        Quaternion hitRotation = Quaternion.FromToRotation(transform.up, vectorToReach); //* transform.rotation;            
        transform.rotation = Quaternion.Slerp(transform.rotation, hitRotation, speed); 
    }*/

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.5f);
    }
}