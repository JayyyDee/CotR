using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActiveBienveillance : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    private void Start()
    {
        Destroy(gameObject, deathTimer);

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != player)
        {
            Vector2 forceDirection = (collider.gameObject.transform.position - gameObject.transform.position)*(-1);
            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 3000f);

            Debug.Log("HIT");
        }
    }
}
