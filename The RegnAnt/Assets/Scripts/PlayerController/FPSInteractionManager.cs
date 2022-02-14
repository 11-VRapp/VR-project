﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSInteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;

    [SerializeField] private Image _target;

    private Interactable _pointingInteractable;
    private Grabbable _pointingGrabbable;

    [SerializeField] private Transform _viewPosition;
    private Vector3 _rayOrigin;

    public Grabbable _grabbedObject = null;
    public Interactable _interactingObject = null;

    public bool feeding = false;
    [SerializeField] private Animator _animator;

    void Update()
    {
        _rayOrigin = _viewPosition.position;
        if (_grabbedObject == null)
            CheckInteraction();

        if (_grabbedObject != null && Input.GetMouseButtonDown(0))
            Drop();

        UpdateUITarget();
    }

    private void CheckInteraction()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _pointingInteractable = hit.transform.GetComponent<Interactable>();
            if (_pointingInteractable)
            {
                if (Input.GetKey(KeyCode.E) && _interactingObject == null)
                {
                    _pointingInteractable.Interact(gameObject);                   
                }                    
            }

            //Check if is grabbable
            _pointingGrabbable = hit.transform.GetComponent<Grabbable>();
            if (_grabbedObject == null && _pointingGrabbable)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _pointingGrabbable.Grab(gameObject);
                    Grab(_pointingGrabbable);
                }

            }

            if(Input.GetMouseButton(0) && hit.transform.GetComponent<Food>() != null)
            {
                _animator.SetTrigger("attack");
                hit.transform.GetComponent<Food>().Eat();
                GetComponent<playerLife>().life += 25f;
                feeding = true;
            }
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingInteractable = null;
            _pointingGrabbable = null;
        }
    }

    private void UpdateUITarget()
    {
        if (_pointingInteractable)
            _target.color = Color.green;
        else if (_pointingGrabbable)
            _target.color = Color.yellow;
        else
            _target.color = Color.red;
    }

    private void Drop()
    {
        if (_grabbedObject == null)
            return;

        _grabbedObject.transform.parent = _grabbedObject.OriginalParent;
        _grabbedObject.Drop();

        _target.enabled = true;
        _grabbedObject = null;
    }

    private void Grab(Grabbable grabbable)
    {
        _grabbedObject = grabbable;
        grabbable.transform.SetParent(_fpsCameraT);

        _target.enabled = false;
    }

    public void Interaction(Interactable interactable) => _interactingObject = interactable;
    
}