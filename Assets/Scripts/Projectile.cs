using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float deathTimer;
    private void Start()
    {
        Destroy(gameObject, deathTimer);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !IsOwner)
        {
            
            Destroy(gameObject);
            Debug.Log("HIT");
            
            //Damage
        }
        

    }
}
