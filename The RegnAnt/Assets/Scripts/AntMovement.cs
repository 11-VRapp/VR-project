using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    [SerializeField] private float _rotateSpeed = 0.1f;
    [SerializeField] Transform orientation = null;
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _moveDirection;       

    [Header("Slope variables")]
    private Vector3 _slopeMoveDirection;
    [SerializeField] private float _playerHeight = 0.25f;
    [SerializeField] private float _stickyForce = 10f;  //fixa il fatto che il player parte verso l'alto tenendolo incollato alla superficie
    RaycastHit slopeHit;

    private Rigidbody _rb;
    private PlayerLook _pl;

    private bool OnSlope()
    {
        Debug.DrawRay(transform.position, -transform.up, Color.green);
        if (Physics.Raycast(transform.position, -transform.up, out slopeHit, _playerHeight / 2 + 2.5f))
            return slopeHit.normal != Vector3.up ? true : false; //cambia... up e down e right e left

        return false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _pl = GetComponent<PlayerLook>();
        _rb.freezeRotation = true;
    }
    private void Update()
    {
        MyInput();

        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, slopeHit.normal);        
    }

    void MyInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        onWall();
    }

    void MovePlayer()
    {
        if (OnSlope()) //non so ma se lo tolgo nel pezzo verticale in alto si stacca, così invece no //o forse devo farlo anche per le altre in onSlope()
        {
            Debug.Log("aaaaa");
            _rb.AddForce(_slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);   
            _rb.AddForce(-transform.up * _stickyForce, ForceMode.Acceleration); //fixa il fatto che il player parte verso l'alto tenendolo incollato alla superficie
        }
        else
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            _rb.AddForce(-transform.up * _stickyForce, ForceMode.Acceleration); //fixa il fatto che il player parte verso l'alto tenendolo incollato alla superficie
        }
    }

    private void onWall()
    {
        //Debug.DrawRay(transform.position, (transform.forward / 3 - transform.up), Color.black);
        //Debug.DrawRay(transform.position, (transform.forward / 3 - transform.up), Color.red);
        //RaycastHit forwardHitWall;
        /*if(Physics.Raycast(transform.position, (transform.forward / 3 - transform.up), out forwardHitWall, 3f))
        {
            Debug.Log("bbbb");
            var hitRotation = Quaternion.FromToRotation(Vector3.up, forwardHitWall.normal);
            transform.rotation = hitRotation;
            _rb.useGravity = false; //riabilitarla quando non è grounded... quindi uno sphere raycast?
        }*/

        if(Physics.SphereCast(transform.position, 1f, -transform.up, out RaycastHit hitWall, 3f) && Physics.SphereCast(transform.position, 1f, -transform.up, out RaycastHit groundHit, 1f)) //doppio cast, uno piccolo per vedere se tocca terra prima di disabilitare gravità
        {
            Debug.Log("GroundDet");
            var hitRotation = Quaternion.FromToRotation(Vector3.up, hitWall.normal);
            //transform.rotation = hitRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, hitRotation, _rotateSpeed);
            _rb.useGravity = false; //riabilitarla quando non è grounded... quindi uno sphere raycast?
        }
        else 
        {
            Debug.Log("WallDet");
            var hitRotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);
            //transform.rotation = hitRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, hitRotation, _rotateSpeed); //transizione smooth, ma in pezzi sottili troppo lenta e fa fare salti... aumentare sticky?
            _rb.useGravity = true;
        }            
    }  

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the hitpoint position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(slopeHit.point, 1f);
    }
   
}
