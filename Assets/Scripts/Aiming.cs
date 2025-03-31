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
        if (!playerCharacter.GetComponent<NetworkObject>().IsOwner)
        {
            return;
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = mousePos - transform.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        SetPlayerAimServerRpc(aimAngle);




    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerAimServerRpc(float aim) {
        
        SetPlayerAimClientRpc(aim);
    }

    [ClientRpc]
    private void SetPlayerAimClientRpc(float aim) {
        
            
           transform.rotation = Quaternion.Euler(0, 0, aim);
        
    }
}


