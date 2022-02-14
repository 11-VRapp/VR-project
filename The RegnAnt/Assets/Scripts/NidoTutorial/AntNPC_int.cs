using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AntNPC_int : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;


    [Header("NPC values")]
    [Range(0, 500)] public float walkRadius;
    [Range(0, 10)] public float _speed;
    [Range(0, 10)] public float _RotateSpeed;
    [SerializeField] Transform orientation = null;
    private bool moving = true;
    private float time;
    private float timeCounter;

    [Header("Slope variables")]
    [SerializeField] private float _gravity = 10f;  //faux gravity: stick player to surface
    private RaycastHit _hitGround;
    [SerializeField] private Transform _groundCheckPosition;
    private Rigidbody _rb;
    [SerializeField] private Animator _animator;
    private Transform _player;

    // Start is called before the first frame update
    void Start() //dare questo script sia a quella del tutorial sia alle altre, ma nelle altre aggiungere uno script con Start => StartCorutine(RandomMovement)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.useGravity = false;
        time = walkRadius / _speed;        
    }

    void FixedUpdate()
    {
        _rb.AddForce(-transform.up * _gravity, ForceMode.Acceleration); //gravità fittizia alle pareti per tenerlo incollato

        if (wallCheck() == false)
            groundCheck();
    }

    public IEnumerator randomMovement()
    {
        while (moving)
        {
            transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(0, Random.Range(-180, 180), 0), 2f);
            yield return new WaitForSeconds(3f);
            yield return moveForward(time);
            yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
        }
    }

    public IEnumerator rotateTowards(float timeNeeded, float speed, Vector3 _target)
    {
        do
        {
            Vector3 dirToTarget = _target - transform.position;
            dirToTarget.y = 0f;
            dirToTarget.Normalize();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirToTarget), speed * Time.deltaTime);
            timeNeeded -= Time.deltaTime;
            yield return null;
        } while (timeNeeded > 0);
    }

    public IEnumerator moveForward(float timeNeeded)
    {
        do
        {
            _rb.velocity = transform.forward * _speed;
            timeNeeded -= Time.deltaTime;
            yield return null;
        } while (timeNeeded > 0);
        timeNeeded = time;
    }

    private void groundCheck() //fattibile con sphere collider?
    {
        Debug.DrawLine(_groundCheckPosition.position + 1f * orientation.forward, _groundCheckPosition.position + 1f * orientation.forward - 4f * transform.up, Color.yellow);
        if (!Physics.SphereCast(_groundCheckPosition.position + 1f * orientation.up, 0.5f, -transform.up, out _hitGround, 3f))
        {
            _rb.useGravity = true;
            return;
        }
        else
        {
            if (_hitGround.collider.gameObject.layer == 6) //terrain layer
            {
                _rb.useGravity = false;
                rotateToSurfaceNormal(_hitGround.normal, _RotateSpeed * Time.deltaTime);
            }
        }
    }

    private bool wallCheck()
    {
        Debug.DrawLine(transform.position - .15f * orientation.up, transform.position - .15f * orientation.up + 2.5f * orientation.forward, Color.black);
        if (Physics.Raycast(transform.position - .15f * orientation.up, orientation.forward, out _hitGround, 2.5f, terrainLayer.value))
        {
            rotateToSurfaceNormal(_hitGround.normal, _RotateSpeed * Time.deltaTime);
            return true;
        }
        return false;
    }

    private void rotateToSurfaceNormal(Vector3 vectorToReach, float speed) => transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, vectorToReach) * transform.rotation, speed);

    public float DistanceFromTarget(Transform _target) => Vector3.Distance(new Vector3(_target.transform.position.x, 0f, _target.transform.position.z), new Vector3(transform.position.x, 0f, transform.position.z));


    void OnDrawGizmosSelected()
    {
    }
}
