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
    public bool canMove = true;  //from spider hook    
    public bool grounded;
    

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

        if(Input.GetKeyDown(KeyCode.LeftShift))
            movementMultiplier = 30f;
        if(Input.GetKeyUp(KeyCode.LeftShift))
            movementMultiplier = 10f;
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
            grounded = groundCheck();
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
        _rb.AddForce(-transform.up * _gravity, ForceMode.Acceleration); //gravit?? fittizia alle pareti per tenerlo incollato
    }

    private bool groundCheck() //fattibile con sphere collider?
    {
        if (!Physics.SphereCast(_groundCheckPosition.position + 1f * orientation.forward, 0.5f, -transform.up, out _hitGround, 4f))
        {
            _rb.useGravity = true;
            return false;
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
        return true;
    }

    private bool wallCheck()
    {
        //Debug.Log("Checking Wall");
        Debug.DrawLine(transform.position - 1f * orientation.up, transform.position - 1f * orientation.up + 2.5f * orientation.forward, Color.black);    
        if (Physics.Raycast(transform.position - 1f * orientation.up, orientation.forward, out _hitGround, 2.5f, terrainLayer.value))
        //|| Physics.Raycast(transform.position - 1f * orientation.up, -orientation.forward, out _hitGround, 2.5f, terrainLayer.value))
        {
            //Debug.LogWarning("Wall detected");            
            rotateToSurfaceNormal(_hitGround.normal, _rotateSpeed * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void rotateToSurfaceNormal(Vector3 vectorToReach, float speed) => transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, vectorToReach) * transform.rotation, speed);

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_hitGround.point, 0.5f);
    }
}
