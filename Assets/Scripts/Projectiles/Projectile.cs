using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    private void Start()
    {
        StartCoroutine(Despawn());
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GetComponent<Rigidbody2D>().AddForce(transform.right * 15, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && collider.gameObject != player)
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
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(deathTimer);
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject, 0f);
    }
}
