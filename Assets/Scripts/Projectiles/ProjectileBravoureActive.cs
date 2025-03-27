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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<PlayerMovement>().SetSlow();
            
            Debug.Log("HIT");
            

            
        }
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerMovement>().SetSlow();

            Debug.Log("LOLXD");



        }


    }
}
