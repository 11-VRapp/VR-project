using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float movementMultiplier = 10f;
    [SerializeField] private float _rotateSpeed = 0.1f;
    [SerializeField] Transform orientation = null;
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _moveDirection;
    private bool _falling = false;

    [Header("Slope variables")]
    [SerializeField] private float _gravity = 10f;  //fixa il fatto che il player parte verso l'alto tenendolo incollato alla superficie
    private Vector3 _slopeMoveDirection;
    private RaycastHit _hitGround;
    [SerializeField] private Transform _groundCheckPosition;
    private Rigidbody _rb;

    [Header("Antennas")]
    [SerializeField] private Transform _headPosition;
    [SerializeField] private Transform _antennaLeft;
    [SerializeField] private Transform _antennaRight;
    [SerializeField] private Transform _antennaLstartPos;
    [SerializeField] private Transform _antennaRstartPos;
    [SerializeField] private Animator _animator;

    public bool canMove = true;  //from spider hook
    [SerializeField] private Transform _grabPosition;
    private bool _isGrabbing = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.useGravity = false;
    }
    private void Update()
    {
        MyInput();
        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _hitGround.normal);

        Grab();
        Attack();
        //GetComponent<PlayerLook>().Interact();
    }

    void MyInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
    }

    private void FixedUpdate()
    {
        if (!canMove)
            return;
        MovePlayer();

        if (wallCheck() == false)
            groundCheck();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _falling = true;
            Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, hitRotation, 0.3f);
        }

        if (_falling)
        {
            _rb.AddForce(-Vector3.up * _gravity, ForceMode.Acceleration);
            moveSpeed = 1f;
        }

        _rb.AddForce(_slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        _rb.AddForce(-transform.up * _gravity, ForceMode.Acceleration); //gravità fittizia alle pareti per tenerlo incollato
    }

    private void groundCheck() //fattibile con sphere collider?
    {
        if (!Physics.SphereCast(_groundCheckPosition.position + 1f * orientation.forward, 0.5f, -transform.up, out _hitGround, 4f))
        {
            _rb.useGravity = true;
            return;
        }
        else
        {
            if (_hitGround.collider.gameObject.layer == 6) //terrain layer
            {
                _rb.useGravity = false;
                _falling = false;
                moveSpeed = 3f;

                rotateToSurfaceNormal(_hitGround.normal, _rotateSpeed * Time.deltaTime);
            }
        }
    }

    private bool wallCheck()
    {
        //Debug.Log("Checking Wall");
        Debug.DrawLine(transform.position - 1f * orientation.up, transform.position - 1f * orientation.up + 2.5f * orientation.forward, Color.black);
        antennasMovement();

        if (Physics.Raycast(transform.position - 1f * orientation.up, orientation.forward, out _hitGround, 2.5f, terrainLayer.value))
        //|| Physics.Raycast(transform.position - 1f * orientation.up, -orientation.forward, out _hitGround, 2.5f, terrainLayer.value))
        {
            //Debug.LogWarning("Wall detected");            
            rotateToSurfaceNormal(_hitGround.normal, _rotateSpeed * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void antennasMovement()
    {
        //Debug.DrawLine(_headPosition.position, _headPosition.position + _headPosition.forward - _headPosition.right * 0.5f, Color.green);
        if (Physics.SphereCast(_headPosition.position, 0.5f, _headPosition.forward - _headPosition.right * 0.5f, out RaycastHit hitL, 1.2f))
        {
            _animator.SetBool("objectClose", true); //animation stop  
            _antennaLeft.position = Vector3.MoveTowards(_antennaLeft.position, hitL.point, Time.fixedDeltaTime);
        }
        else
            _antennaLeft.position = Vector3.MoveTowards(_antennaLeft.position, _antennaLstartPos.position, Time.fixedDeltaTime);


        //Debug.DrawLine(_headPosition.position, _headPosition.position + _headPosition.forward + _headPosition.right * 0.5f, Color.red);
        if (Physics.SphereCast(_headPosition.position, 0.5f, _headPosition.forward + _headPosition.right * 0.5f, out RaycastHit hitR, 1.2f))
        {
            _animator.SetBool("objectClose", true); //animation stop  

            _antennaRight.position = Vector3.MoveTowards(_antennaRight.position, hitR.point, Time.fixedDeltaTime);
        }
        else
            _antennaRight.position = Vector3.MoveTowards(_antennaRight.position, _antennaRstartPos.position, Time.fixedDeltaTime);

        if (Vector3.Distance(_antennaLeft.position, _antennaLstartPos.position) < 0.005f && Vector3.Distance(_antennaRight.position, _antennaRstartPos.position) < 0.005f)
            _animator.SetBool("objectClose", false);
    }

    private void rotateToSurfaceNormal(Vector3 vectorToReach, float speed) => transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, vectorToReach) * transform.rotation, speed);

    private void Grab()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!_isGrabbing)
            {
                RaycastHit obj = GetComponent<PlayerLook>().hit;

                if (obj.collider != null)
                {
                    _animator.SetBool("grab", true);
                    GameObject pivot = new GameObject("pivotPoint");
                    pivot.transform.position = obj.point;

                    obj.collider.transform.SetParent(pivot.transform);
                    pivot.transform.SetParent(_grabPosition.transform);
                    pivot.transform.localPosition = Vector3.zero;

                    // _headController_target.DOLocalMove(_headController_target_startPosition, 2f);

                    _isGrabbing = true;
                }
            }
            else
            {
                if (_grabPosition.childCount != 0)
                {
                    _animator.SetBool("grab", false);
                    Transform pivot = _grabPosition.GetChild(0);
                    //pivot.GetChild(0).parent = null;
                    pivot.parent = null;
                    pivot.gameObject.AddComponent<FoodManager>();

                    _isGrabbing = false;
                }
            }
        }
    }


    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("attack");
            /*RayHit obj = GetComponent<PlayerLook>().hit;
            if (obj.collider != null)
            {

            }*/
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_hitGround.point, 0.5f);

        /*Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 0.5f);*/

        /*Gizmos.color = Color.green;
        Gizmos.DrawSphere(_antennaRstartPos.position, 0.2f);
        Gizmos.DrawSphere(_antennaLstartPos.position, 0.2f);*/

        /*Gizmos.color = Color.green;
        Gizmos.DrawSphere(hitL.point, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitR.point, 0.2f);*/
    }
}
