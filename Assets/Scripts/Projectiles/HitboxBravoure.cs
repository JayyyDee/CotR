using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxBravoure : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);    
        }
    }
}
