using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float deathTimer;
    private void Start()
    {
        Destroy(gameObject, deathTimer);
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Dummy"))
        {
            Destroy(gameObject);
            //Damage
        }
        

    }
}
