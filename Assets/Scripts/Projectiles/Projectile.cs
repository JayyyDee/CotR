using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    private void Start()
    {
        Destroy(gameObject, deathTimer);
    }
   
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(gameObject);
            Debug.Log("HIT");
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
            
            //Damage
        }
        if(collider.gameObject.CompareTag("Walls")){
            Destroy(gameObject);
        }
        

    }
}
