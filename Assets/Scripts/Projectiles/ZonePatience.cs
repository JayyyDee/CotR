using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZonePatience : NetworkBehaviour
{
    public int damage;
    public int healing;
    public GameObject player;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    public void Explode()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && collider.gameObject == player)
        {
            collider.gameObject.GetComponent<HealthManager>().HealingServerRpc(healing);
            Debug.Log("HEAL");
        }
        if (collider.CompareTag("Enemy") && collider.gameObject != player)
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
            Debug.Log("HEAL");
        }
    }
}
