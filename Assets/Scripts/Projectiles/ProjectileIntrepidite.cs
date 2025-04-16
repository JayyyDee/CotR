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
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        StartCoroutine(Despawn(deathTimer));
        GetComponent<Rigidbody2D>().AddForce(transform.right * 15, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy") && collider.gameObject != NetworkManager.LocalClient.PlayerObject && IsOwner)
        {    
            Debug.Log("HIT");
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
            DespawnServerRpc();
        }
        if(collider.gameObject.CompareTag("Walls")){
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
        DespawnClientRpc();
    }

    [ClientRpc]
    public void DespawnClientRpc() {
        
        Destroy(gameObject);

    }
}
