using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antennas : MonoBehaviour
{

    [Header("Antennas")]
    [SerializeField] private Transform _headPosition;
    [SerializeField] private Transform _antennaLeft;
    [SerializeField] private Transform _antennaRight;
    [SerializeField] private Transform _antennaLstartPos;
    [SerializeField] private Transform _antennaRstartPos;
    [SerializeField] private Animator _animator;

    void Update() => antennasMovement();

    private void antennasMovement()
    {
        //Debug.DrawLine(_headPosition.position, _headPosition.position + _headPosition.forward - _headPosition.right * 0.5f, Color.green);
        if (Physics.SphereCast(_headPosition.position, 0.2f, _headPosition.forward - _headPosition.right * 0.5f, out RaycastHit hitL, 1.1f))        
            _antennaLeft.position = Vector3.MoveTowards(_antennaLeft.position, hitL.point, Time.fixedDeltaTime);        
        else
            _antennaLeft.position = Vector3.MoveTowards(_antennaLeft.position, _antennaLstartPos.position, Time.fixedDeltaTime);

        //Debug.DrawLine(_headPosition.position, _headPosition.position + _headPosition.forward + _headPosition.right * 0.5f, Color.red);
        if (Physics.SphereCast(_headPosition.position, 0.2f, _headPosition.forward + _headPosition.right * 0.5f, out RaycastHit hitR, 1.1f)) 
            _antennaRight.position = Vector3.MoveTowards(_antennaRight.position, hitR.point, Time.fixedDeltaTime);        
        else
            _antennaRight.position = Vector3.MoveTowards(_antennaRight.position, _antennaRstartPos.position, Time.fixedDeltaTime);
    }
}
