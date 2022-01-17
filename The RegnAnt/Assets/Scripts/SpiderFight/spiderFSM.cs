using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;


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

    [SerializeField] private List<GameObject> _legRigs;
    [SerializeField] private List<FootIKSolver> _legIKSolver;

    private int _attackPhase = 0;

    private List<int> legsAttacking;

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


        legsAttacking = new List<int>();
    }

    // Update is called once per frame
    void Update() => _stateMachine.Tik();

    public void SetRandomPointDestination()
    {
        //Vector3 destination;
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
    
    private Vector3 direction;
    public void RotateTowards()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance + 15f)
        {
            direction = (_terrain.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * 5f);
        }
    }

    public void setUpAttack()
    {
        /* Choosing legs preparing to attack */
        int z_sx = Random.Range(1, 3); //maxExclusive        

        Debug.Log(z_sx);

        for (int i = 2; i <= 6; i += 2)  // Only one leg on the left side  // so get central one on sx, others but non central on dx     
            legsAttacking.Add(i - z_sx); //indexes of legsRigs relative to the legs chosen

        legsAttacking.Add(Random.Range(1, 3) + 5); //one of the two frontal legs

        /* disable legIkSolver to elevate the leg */

        foreach (int legIndex in legsAttacking)
        {
            _legIKSolver[legIndex].enabled = false;
            _legRigs[legIndex].GetComponent<MultiRotationConstraint>().weight = 1;
            //_legRigs[legIndex].GetComponent<TwoBoneIKConstraint>().weight = 0;
            //_legRigs[legIndex].GetComponent<ChainIKConstraint>().weight = 1;
        }
    }

    public void AttackWithLegs()
    {
        switch (_attackPhase)
        {
            case 0:
                RiseLegs();
                break;
            case 1:
                LookingNearestObject();
                break;
        }
    }


    private void RiseLegs()
    {
        foreach (int legIndex in legsAttacking)
        {
            _legIKSolver[legIndex].transform.position = Vector3.MoveTowards(_legIKSolver[legIndex].transform.position, new Vector3(_legIKSolver[legIndex].transform.position.x, 11f, _legIKSolver[legIndex].transform.position.z), 3f * Time.deltaTime);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, new Vector3(_legIKSolver[legIndex].transform.position.x, 11f, _legIKSolver[legIndex].transform.position.z)) < 0.001f)
            {
                _attackPhase = 1;
                return;
            }
        }
    }
    
    private void LookingNearestObject()
    {
       foreach (int legIndex in legsAttacking)
        {
            _legIKSolver[legIndex].GetComponent<SphereCollider>().enabled = true;
            //_legRigs[legIndex].GetComponent<MultiRotationConstraint>().weight = 1;
            //_legRigs[legIndex].GetComponent<TwoBoneIKConstraint>().weight = 0;
            //_legRigs[legIndex].GetComponent<ChainIKConstraint>().weight = 1;
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(direction, 5f);
        
    }
}
