using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActivePerseverance : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    private void Start()
    {
        StartCoroutine(Despawn(deathTimer));
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {   // && collider.gameObject != player && player.GetComponent<NetworkObject>().IsOwner
        if (collider.CompareTag("Enemy") && collider.gameObject != player && player.GetComponent<NetworkObject>().IsOwner)
        {
            Vector2 forceDirection = (collider.gameObject.transform.position - gameObject.transform.position).normalized;
            
            collider.gameObject.GetComponent<PlayerMovement>().AddForceServerRpc(forceDirection, 150f); 
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
