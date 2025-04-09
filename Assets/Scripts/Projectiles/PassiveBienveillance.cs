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
        gameObject.transform.position = player.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.transform.position);
        Debug.Log(gameObject.transform.position);
        //gameObject.transform.position = player.transform.position;
        MoveZoneServerRpc();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && collider.gameObject != player)
        {
            collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc((int)(damage * Time.deltaTime));
            Debug.Log("Dmg");
        }
    }


    [ServerRpc]
    public void MoveZoneServerRpc( )
    {
        MoveZoneClientRpc();
    }

    [ClientRpc]
    public void MoveZoneClientRpc()
    {
        //gameObject.transform.position = player.transform.position;
    }
}
