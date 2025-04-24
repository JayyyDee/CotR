using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PassiveBienveillance : NetworkBehaviour
{
    public int damage;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (NetworkManager.LocalClient.PlayerObject)
        {
            Vector2 playerPos = NetworkManager.LocalClient.PlayerObject.transform.position;
            MoveZoneServerRpc(playerPos);
        }
        else
        {
            TurnOffServerRpc();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != NetworkManager.LocalClient.PlayerObject && IsOwner)
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);
            Debug.Log("Dmg");
        }

    }


    [ServerRpc(RequireOwnership = false)]
    public void MoveZoneServerRpc(Vector2 pos)
    {

        MoveZoneClientRpc(pos);
    }

    [ClientRpc]
    public void MoveZoneClientRpc(Vector2 pos)
    {
        gameObject.transform.position = pos;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TurnOffServerRpc()
    {
        gameObject.GetComponent<NetworkObject>().Despawn();
    }
}
