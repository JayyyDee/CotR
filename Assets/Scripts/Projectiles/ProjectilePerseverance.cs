using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePerseverance : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    public float force;
    private void Start()
    {
        StartCoroutine(Despawn(deathTimer));
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GetComponent<Rigidbody2D>().AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && collider.gameObject != player && player.GetComponent<NetworkObject>().IsOwner)
        {
            DespawnServerRpc();
            Debug.Log("HIT");
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
        }
        if (collider.gameObject.CompareTag("Walls"))
        {
            DespawnServerRpc();
        }


    }
    IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);
        DespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);

    }
}
