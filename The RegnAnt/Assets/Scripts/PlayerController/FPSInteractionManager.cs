using UnityEngine;
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
    private Damagable _pointingDamagable;

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

        Debug.DrawRay(_rayOrigin, _fpsCameraT.forward, Color.red);

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
            
            if (Input.GetMouseButton(0) && !_animator.GetBool("hit"))
            {                        
                _animator.SetBool("hit", true);                
                if (hit.transform.GetComponent<Food>() != null)
                {
                    hit.transform.GetComponent<Food>().Eat();
                    GetComponent<playerLife>().setHeal(25f);
                    feeding = true;
                    GetComponent<AudioManager>().Play("Eat");
                }

                _pointingDamagable = hit.collider.transform.GetComponent<Damagable>();
                if (_pointingDamagable && GetComponent<AntMovement>().canMove) //spiderHit (not if you are grabbed)   
                {
                    _pointingDamagable.Hit();  //damage SPIDER  
                    GetComponent<AudioManager>().Play("Attack");
                }                            
                    
            }
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingInteractable = null;
            _pointingGrabbable = null;
            _pointingDamagable = null;
        }
    }

    private void UpdateUITarget()
    {
        if (_pointingInteractable)
            _target.color = Color.green;
        else if (_pointingGrabbable)
            _target.color = Color.blue;
        else if (_pointingDamagable)
            _target.color = Color.yellow;
        else
            _target.color = Color.red;
    }

    public void Drop()
    {
        if (_grabbedObject == null)
            return;

        _grabbedObject.transform.parent = _grabbedObject.OriginalParent;
        _grabbedObject.Drop();

        _target.enabled = true;
        _grabbedObject = null;
        GetComponent<AudioManager>().Play("Drop");
    }

    public void Grab(Grabbable grabbable)
    {
        _grabbedObject = grabbable;
        grabbable.transform.SetParent(_fpsCameraT);

        _target.enabled = false;

        GetComponent<AudioManager>().Play("Grab");
    }

    public void Interaction(Interactable interactable) => _interactingObject = interactable;

}
