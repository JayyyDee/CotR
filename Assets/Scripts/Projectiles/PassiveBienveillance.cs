using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PassiveBienveillance : NetworkBehaviour
{
    public int damage;
    public GameObject player;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos = player.transform.position;
        MoveZoneServerRpc(playerPos);
    }

    private void OnTriggerStay2D(Collider2D collider)
    { 
        if (collider.CompareTag("Enemy") && collider.gameObject != player)
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
}
