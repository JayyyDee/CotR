using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Aiming : NetworkBehaviour
{
    private Vector3 mousePos;
    public Camera cam;
    
    

    void FixedUpdate()
    {
        if (IsOwner)
        {
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 aimDirection = mousePos - transform.position;
            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, aimAngle); 
        }
        
        //SetPlayerAimServerRpc();
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void SetPlayerAimServerRpc() {
    //    SetPlayerAimClientRpc();
    //}

    //[ClientRpc]
    //private void SetPlayerAimClientRpc() {

    //    //if (!IsOwner) {
    //    //    transform.rotation = Quaternion.Euler(0, 0, aim);
    //    //}
        

    //}
}
