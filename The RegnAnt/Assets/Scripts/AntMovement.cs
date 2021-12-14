using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntMovement : MonoBehaviour
{
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
    RaycastHit hitWall;

    [SerializeField] private Transform _groundCheckPosition;
    

    private Rigidbody _rb;        

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();            
        _rb.freezeRotation = true;
        _rb.useGravity = false;
    }
    private void Update()
    {
        MyInput();

        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, hitWall.normal);        
    }

    void MyInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
    }

    private void FixedUpdate()
    {
        groundCheck();
        MovePlayer();        
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.Space)) 
        {           
            _falling = true;            
            var hitRotation = Quaternion.FromToRotation(Vector3.up, Vector3.up);            
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

    private void groundCheck()
    {     
        if(Physics.SphereCast(_groundCheckPosition.position, 1f, -transform.up, out hitWall, 3f)) 
        {
            _falling = false;
            moveSpeed = 3f;

            Debug.Log("GroundDet " + hitWall.transform.gameObject.name);
            var hitRotation = Quaternion.FromToRotation(Vector3.up, hitWall.normal);            
            transform.rotation = Quaternion.Slerp(transform.rotation, hitRotation, _rotateSpeed);  
        }                   
    }  

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_groundCheckPosition.position, 1f);
    }
}
