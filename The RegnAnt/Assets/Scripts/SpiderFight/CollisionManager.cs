using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private spiderFSM _spiderFSM;
    [SerializeField] private List<Collider> legsColliders;
    [SerializeField] private List<Collider> mandiboleColliders;

    [SerializeField] private float _legDamage = 15f;
    [SerializeField] private float _mandDamagePerSecond = 15f;

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

            if(legsColliders.Contains(collision.contacts[0].thisCollider)) //se è una zampa
            {
                if(_spiderFSM.collidingWithAttackingLeg(legsColliders.FindIndex(d => d == collision.contacts[0].thisCollider)))  //in questo modo se provi a salire su gamba a terra dopo attacco non vieni ferito
                    collision.gameObject.GetComponent<playerLife>().setDamage(_legDamage);
                Debug.LogWarning("Gamba");
                return;
            }

            if(mandiboleColliders.Contains(collision.contacts[0].thisCollider)) //se è una mandibola
            {
                Debug.LogWarning("Mandibola");
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
}
