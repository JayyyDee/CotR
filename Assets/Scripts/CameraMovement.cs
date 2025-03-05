using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraMovement : MonoBehaviour
  
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Locks the camera on the owner of the game only.
        //if (!IsOwner)
        //{
        //    return;
        //}

        transform.position = player.transform.position + new Vector3(0,0,-5);
    }
}
