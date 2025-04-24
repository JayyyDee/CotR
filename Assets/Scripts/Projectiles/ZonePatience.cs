using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZonePatience : NetworkBehaviour
{
    public int explosionDamage;
    public int damage;
    public int healing;
    private bool explode= false;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void Detonate()
    {
        DetonateServerRpc();
        explode = true;
        StartCoroutine(Despawn());
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")  && IsOwner)
        {
            
            collider.gameObject.GetComponent<HealthManager>().HealingServerRpc((int)(healing* Time.deltaTime));
            
        }
        if (collider.CompareTag("Enemy") && collider.gameObject != NetworkManager.LocalClient.PlayerObject && IsOwner)
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc((int)(damage * Time.deltaTime));
     
        }
        if (explode)
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(explosionDamage);
            explode = false;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DetonateServerRpc()
    {
        DetonateClientRpc();
        
    }

    [ClientRpc]
    private void DetonateClientRpc()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject, 0f);
        explode = false;
    }

}
