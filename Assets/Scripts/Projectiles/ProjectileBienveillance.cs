using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileBienveillance : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    public float force = 15f;
    public float scale = 1f;
    private void Start()
    {
        Destroy(gameObject, deathTimer);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        GetComponent<Rigidbody2D>().AddForce(transform.right *force,ForceMode2D.Impulse);
    }

    private void Update()
    {
        transform.localScale += new Vector3(scale * Time.deltaTime*2, scale * Time.deltaTime*8);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != player)
        {

            Destroy(gameObject);
            Debug.Log("HIT");
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);

            //Damage
        }
    }
}
