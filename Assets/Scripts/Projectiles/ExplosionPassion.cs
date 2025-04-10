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
        StartCoroutine(Despawn(0.1f));
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
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
        //DespawnClientRpc();
    }
}
