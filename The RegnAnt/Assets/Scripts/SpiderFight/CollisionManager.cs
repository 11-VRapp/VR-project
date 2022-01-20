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
            Debug.LogWarning("name: " + name + "   " + collision.gameObject.name + "    Player HIT ");
            Debug.Log(collision.contacts[0].thisCollider.name);

            if (legsColliders.Contains(collision.contacts[0].thisCollider)) //se è una zampa
            {
                if (_spiderFSM.collidingWithAttackingLeg(legsColliders.FindIndex(d => d == collision.contacts[0].thisCollider)))  //in questo modo se provi a salire su gamba a terra dopo attacco non vieni ferito
                    _player.GetComponent<playerLife>().setDamage(_legDamage);
                    //collision.gameObject.GetComponent<playerLife>().setDamage(_legDamage);
                Debug.LogWarning("Gamba");
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


            /*foreach (ContactPoint c in collision.contacts)
            {
                Debug.Log(c.thisCollider.name);
            }*/
            return;
        }


        if (collision.gameObject.layer == 9) // ANT layer
        {

            Debug.LogWarning("Ant npc|player hit");
        }
    }

    public IEnumerator timerHookingInJaws()
    {
        for(int i = 0; i < 5 && _getFreeAttack < _nAttacksToFree; i++)
        {            
            yield return new WaitForSeconds(1f);
            _player.GetComponent<playerLife>().setDamage(_mandDamagePerSecond);
        }
        /* Unset parent: free the player*/
        _player.parent = null;
        //or play launch animation
        GetComponent<Animator>().SetBool("launch", true);

        _player.GetComponent<Rigidbody>().AddForce(3500f * (4 * _spiderFSM.transform.forward + Vector3.up), ForceMode.Acceleration);
        _player.GetComponent<AntMovement>().canMove = true;
        if(_getFreeAttack > _nAttacksToFree)
            yield return new WaitForSeconds(3f);
        _spiderFSM.JawsHooking = false;
        _getFreeAttack = 0;
        _nAttacksToFree++;
        GetComponent<Animator>().SetBool("launch", false);
        yield return null;
    }

    void Update() //just temporary, to free. Attack ant not implemented yet
    {
        if(Input.GetMouseButtonDown(0)) 
        {
           _getFreeAttack++;
        }
    }
}
