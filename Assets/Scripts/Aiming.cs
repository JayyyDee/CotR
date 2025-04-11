using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Netcode;
using UnityEngine;

public class Aiming : NetworkBehaviour {
    private Vector3 mousePos;
    public Camera cam;
    public GameObject playerCharacter;
   

    void FixedUpdate() {
        if (!IsOwner)
        {
            return;
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = mousePos - transform.position;
        float aim = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, aim);
        SetPlayerAimServerRpc(aim);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerAimServerRpc(float aim) {
        SetPlayerAimClientRpc(aim);
    }

    [ClientRpc]
    private void SetPlayerAimClientRpc(float aim) {
        transform.rotation = Quaternion.Euler(0, 0, aim);

    }

    public void SetPlayer(GameObject player)
    {
        playerCharacter = player;
    }
}


