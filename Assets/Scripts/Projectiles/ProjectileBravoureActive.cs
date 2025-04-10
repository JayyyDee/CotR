using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileBravoureActive : NetworkBehaviour
{
    public float deathTimer;
    public int damage;
    public GameObject player;
    private void Start()
    {
        AkUnitySoundEngine.PostEvent("Play_Anneaux_Bravoure_Actif_Absorb_Convert_Full__itemnumber", this.gameObject);
        StartCoroutine(Despawn());

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != player && player.GetComponent<NetworkObject>().IsOwner)
        {
            collider.gameObject.GetComponent<PlayerMovement>().SlowServerRpc();
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(deathTimer);
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject, 0f);
    }
}
        
        
