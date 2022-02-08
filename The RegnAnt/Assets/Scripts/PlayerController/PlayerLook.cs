using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerLook : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation = null;
    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    private float xRotation;
    private float yRotation;

    [SerializeField] private Transform _viewPosition;

    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject cursor_selected;
    private Outline _lastHit;
    public RaycastHit hit;

    //public bool interactng = false; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, cam.transform.rotation.z);
        orientation.transform.localEulerAngles = new Vector3(orientation.transform.localEulerAngles.x, yRotation, orientation.transform.localEulerAngles.z);

        detectLookingObject();
    }

    private void detectLookingObject()
    {
        Debug.DrawLine(_viewPosition.position, _viewPosition.position + 2 * _viewPosition.forward, Color.magenta);
        if (Physics.SphereCast(_viewPosition.position, 0.2f, _viewPosition.forward, out hit, 1f))
        {
            if (_lastHit != null && hit.transform != _lastHit.transform)
            {
                _lastHit.outlineWidth = 1f;
                _lastHit.UpdateMaterialProperties();
            }

            _lastHit = hit.collider.GetComponent<Outline>();
            if (_lastHit != null)
            {
                cursor.SetActive(false);
                cursor_selected.SetActive(true);

                _lastHit.outlineWidth = 10f;
                _lastHit.UpdateMaterialProperties();
            }
            return;
        }

        if (hit.collider == null)
        {
            cursor.SetActive(true);
            cursor_selected.SetActive(false);
        }
    }

    public void Interact()
    {
        Debug.DrawLine(_viewPosition.position, _viewPosition.position + 2 * _viewPosition.forward, Color.magenta);
        if (Physics.SphereCast(_viewPosition.position, 0.2f, _viewPosition.forward, out RaycastHit _hit, 2f, LayerMask.GetMask("Ant")))
        {            
            if(Input.GetKey(KeyCode.E))
            {
                Debug.Log(_hit.transform.name);
                /*_hit.transform.GetComponent<NavMeshAgent>().speed = 0; //non farlo qua. Lo passo al triggerDialogue?
                _hit.transform.DORotate(new Vector3(transform.position.x, 0f, transform.position.z), 2f);*/
                _hit.transform.GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }

    }
}