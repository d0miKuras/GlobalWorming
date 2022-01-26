using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float antDamage = 15.0f;

    void OnTriggerStay(Collider other)
    {
        //player invincibility handled in Health script
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Health>().TakeDamage(antDamage);
        }
    }
}
