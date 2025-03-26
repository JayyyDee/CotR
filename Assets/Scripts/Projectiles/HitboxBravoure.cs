using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBravoure : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (collision.collider.CompareTag("Enemy"))
        //{

            
            Debug.Log("HIT");
            collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);

            //Damage
        //}


    }
}
