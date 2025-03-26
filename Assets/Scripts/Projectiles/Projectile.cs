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
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            
            Destroy(gameObject);
            Debug.Log("HIT");
            collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
            
            //Damage
        }
        

    }
}
