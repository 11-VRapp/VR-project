using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private spiderFSM _spiderFSM;
    [SerializeField] private Transform _player;
    [SerializeField] private List<Collider> legsColliders;
    [SerializeField] private List<Collider> mandiboleColliders;

    [SerializeField] private float _legDamage = 15f;
    [SerializeField] private float _mandDamagePerSecond = 10f;
    [SerializeField] private Transform _jawHookPosition;
    [SerializeField] private int _getFreeAttack = 0;
    [SerializeField] private int _nAttacksToFree = 2;

    private void Start()
    {
        _spiderFSM = GetComponent<spiderFSM>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.LogWarning("name: " + name + "   " + collision.gameObject.name + "    Player HIT ");
            //Debug.Log(collision.contacts[0].thisCollider.name);

            if (legsColliders.Contains(collision.contacts[0].thisCollider)) //se è una zampa
            {
                if (_spiderFSM.collidingWithAttackingLeg(legsColliders.FindIndex(d => d == collision.contacts[0].thisCollider)))  //in questo modo se provi a salire su gamba a terra dopo attacco non vieni ferito
                    _player.GetComponent<playerLife>().setDamage(_legDamage);
                //collision.gameObject.GetComponent<playerLife>().setDamage(_legDamage);
                //Debug.LogWarning("Gamba");
                return;
            }

            if (mandiboleColliders.Contains(collision.contacts[0].thisCollider)) //se è una mandibola
            {
                Debug.LogWarning("Mandibola");
                /* Aggancio e stop attacking con mandibole*/
                _spiderFSM.JawsHooking = true;
                _player.GetComponent<AntMovement>().canMove = false;
                /* diventa figlio e localposition reset*/
                //_player.SetParent(_jawHookPosition.parent);
                _player.transform.parent = _jawHookPosition.transform;
                _player.localPosition = Vector3.zero;
                StartCoroutine(timerHookingInJaws());

                return;
            }           
            return;
        }
        
        if (collision.gameObject.layer == 9) // ANT layer
        {
            collision.transform.GetComponent<antNPCfight>().setDamage(8f);
            collision.transform.GetComponent<Rigidbody>().AddForce(500 * collision.contacts[0].normal, ForceMode.Acceleration);
        }
    }

    public IEnumerator timerHookingInJaws()
    {
        for (int i = 0; i < 5 && _getFreeAttack < _nAttacksToFree; i++)
        {
            yield return new WaitForSeconds(1f);
            _player.GetComponent<playerLife>().setDamage(_mandDamagePerSecond);
        }
        /* Unset parent: free the player*/
        _player.parent = null;
        //or play launch animation
        GetComponent<Animator>().SetTrigger("launch");

        _player.GetComponent<Rigidbody>().AddForce(3500f * (4 * _spiderFSM.transform.forward + Vector3.up), ForceMode.Acceleration);
        _player.GetComponent<AntMovement>().canMove = true;
       
        _spiderFSM.JawsHooking = false;
        _getFreeAttack = 0;
        _nAttacksToFree+=5;
        
        yield return null;
    }

    void Update() 
    {
        if (_spiderFSM.JawsHooking && Input.GetMouseButtonDown(0))        
            _getFreeAttack++;        

        if (_spiderFSM.JawsHooking && _getFreeAttack > _nAttacksToFree)
        {
            GetComponent<Animator>().SetTrigger("launch");
            _spiderFSM.JawsHooking = false;
            _getFreeAttack = 0;
            _nAttacksToFree+=5;
            StopAllCoroutines();

            _player.parent = null;
            _player.GetComponent<Rigidbody>().AddForce(350f * (4 * _spiderFSM.transform.forward + Vector3.up), ForceMode.Acceleration);
            _player.GetComponent<AntMovement>().canMove = true;

        }

    }
}
