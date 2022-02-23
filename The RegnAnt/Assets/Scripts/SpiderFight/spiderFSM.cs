using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class spiderFSM : MonoBehaviour
{
    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius = 250;

    [SerializeField] private float _minDistance = 150;
    private FiniteStateMachine<spiderFSM> _stateMachine;
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _terrain;
    Vector3 destination;

    [SerializeField] private List<GameObject> _legRigs;
    [SerializeField] private List<FootIKSolverSpider> _legIKSolver;

    [SerializeField] private Transform _player;
    [SerializeField] [Range(0, 20)] private float _warningDistance;
    [SerializeField] [Range(0, 20)] private float _biteDistance;
    private int sem_leg_counter;
    private List<int> legsAttacking;
    private bool _waitingToRe_pose = false;
    [SerializeField] private Transform _headTarget;
    private Animator _animator;
    public float offset = 0;
    public bool moving = true;

    [SerializeField] private int _maxAttacksPerRound = 3;
    private int attackCounter = 1;

    public bool JawsHooking = false;

    public bool test = false;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _stateMachine = new FiniteStateMachine<spiderFSM>(this);

        //STATES
        State runState = new RunState("Run", this);
        State attackState = new AttackState("Attack", this);

        //TRANSITIONS
        _stateMachine.AddTransition(runState, attackState, () => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance);        
        _stateMachine.AddTransition(attackState, runState, () => attackCounter == _maxAttacksPerRound); //counter of legs

        //START STATE
        _stateMachine.SetState(runState); //runState


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
            //print(randomPosition + "   " + NavMesh.SamplePosition(randomPosition, out hit, walkRadius, 1) + "   " + Vector3.Distance(transform.position, hit.position)+ "  " + (Vector3.Distance(transform.position, hit.position) < _minDistance));
        } while (!NavMesh.SamplePosition(randomPosition, out hit, walkRadius, 1) || Vector3.Distance(transform.position, hit.position) < _minDistance);

        finalPosition = hit.position;

        return finalPosition;
    }

    private Vector3 _direction;    
    public void StartAttack()
    {
        _maxAttacksPerRound = Random.Range(2, 5);
        StartCoroutine(AttackManager());
    }
    private IEnumerator AttackManager()
    {
        for (attackCounter = 0; attackCounter < _maxAttacksPerRound; attackCounter++)
        {
            //Debug.Log("Counter attack: " + attackCounter + "/" + _maxAttacksPerRound);
            sem_leg_counter = 0;
            StartCoroutine(Attack());
            yield return new WaitUntil(() => (sem_leg_counter == 4));

            //yield return new WaitForSeconds(2f);   
        }

        yield return null;
    }

    private IEnumerator Attack()
    {
        foreach (FootIKSolverSpider leg in _legIKSolver)
            leg.enabled = true;
        legsAttacking.Clear();

        /* Choosing legs preparing to attack */
        int z_sx = Random.Range(1, 3); //maxExclusive        

        //print("z_sx: " + z_sx);

        for (int i = 2; i <= 6; i += 2)  // Only one leg on the left side  // so get central one on sx, others but non central on dx     
            legsAttacking.Add(i - z_sx); //indexes of legsRigs relative to the legs chosen

        legsAttacking.Add(Random.Range(1, 3) + 5); //one of the two frontal legs

        /* disable legIkSolver to elevate the leg */
        foreach (int legIndex in legsAttacking)
            StartCoroutine(AttackCoroutine(_legIKSolver[legIndex], _legRigs[legIndex]));

        yield return null;
    }
    private IEnumerator AttackCoroutine(FootIKSolverSpider leg, GameObject legParent)
    {
        leg.enabled = false;
        legParent.GetComponent<MultiRotationConstraint>().weight = 1;

        while (leg.transform.position.y < 10f)
        {
            leg.transform.Translate(Vector3.up * 3f * Time.deltaTime, Space.World);
            yield return null;
        }

        //leg.transform.DOShakePosition(3f, .6f, 2, 10);
        yield return new WaitForSeconds(.5f);

        Transform target = leg.GetComponent<footController>().CheckEnemyToAttack();
        if (target != null)
        {
            re_Posing(true);
            StartCoroutine(leg.GetComponent<footController>().LegAttack(new Vector3(target.position.x, 0f, target.position.z)));
        }

        yield return new WaitForSeconds(.5f);
        legsAttacking.Clear(); //in questo modo se provi a salire su gamba a terra dopo attacco non vieni ferito
        yield return new WaitForSeconds(2f);
        
        re_Posing(false);

        legParent.GetComponent<MultiRotationConstraint>().weight = 0;
        leg.enabled = true;
        sem_leg_counter++;
    }

    public void CheckPlayerNotTooClose()
    {
        if(JawsHooking)
        {
            _animator.SetBool("AttackZone", false);            
            return;
        }

        if (Vector3.Distance(transform.position, _player.position) > _biteDistance)
            _animator.SetBool("AttackZone", false);

        if (Vector3.Distance(transform.position, _player.position) < _warningDistance && !_waitingToRe_pose) //NON re-posing nell'istante in cui si è fermato dopo attacco!!
        {
            //Debug.LogWarning("Too CLOSE!");
            Vector3 targetDirection = _player.transform.position - transform.position;
            targetDirection.y = 0f;
            targetDirection.Normalize();

            //Rotate toward target direction
            float rotationStep = 2f * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection, transform.up);


            //_headTarget.transform.position = _player.position - Vector3.up * offset; //DOMove?
            _headTarget.transform.DOMove(_player.position - Vector3.up * 2f, 1f);

            if (Vector3.Distance(transform.position, _player.position) < _biteDistance)
                _animator.SetBool("AttackZone", true);

            return;
        }
        else
        _headTarget.transform.DOLocalMove(new Vector3(0f, 1.7f, 0f), 3f);
            //_headTarget.transform.localPosition = new Vector3(0f, 1.7f, 0f);
            
    }    

    public void re_Posing(bool p) => _waitingToRe_pose = p;
    public void SetSemaphoreLegsCounter(int cnt) => sem_leg_counter = cnt;
    public bool collidingWithAttackingLeg(int index) => legsAttacking.Contains(index);    

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the destination point
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 5f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_direction, 5f);

        Debug.DrawLine(transform.position, transform.position + Vector3.forward * _warningDistance, Color.yellow);
        Debug.DrawLine(transform.position, transform.position + Vector3.forward * _biteDistance, Color.red);
    }
}
