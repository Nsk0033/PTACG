using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGasDamage : MonoBehaviour
{
    [SerializeField] private float poisonDamage;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            //health = GetComponent<Health>();
        }
    }

}
