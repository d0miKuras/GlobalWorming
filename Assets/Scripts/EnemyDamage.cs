using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float antDamage;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Health>().TakeDamage(antDamage);
        }
    }
}
