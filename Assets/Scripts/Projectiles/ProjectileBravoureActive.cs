using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBravoureActive : MonoBehaviour
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
            collision.gameObject.GetComponent<PlayerMovement>().SetSlow();
            
            Debug.Log("HIT");
            

            
        }
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().SetSlow();

            Debug.Log("LOLXD");



        }


    }
}
