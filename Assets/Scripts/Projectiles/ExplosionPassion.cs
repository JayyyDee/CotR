using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ExplosionPassion : NetworkBehaviour
{

    public int damage;
    private void Start()
    {
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Passion_Anneaux_Passion_Attack_Pt2__itemnumber", this.gameObject);
        StartCoroutine(Despawn());
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            DespawnServerRpc();
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.1f);
        DespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
        DespawnClientRpc();
    }

    [ClientRpc]
    public void DespawnClientRpc()
    {

        Destroy(gameObject, 0.1f);
    }
}
