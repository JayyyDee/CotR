using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraMovement : MonoBehaviour
  
{
     [SerializeField] public Transform player;
    public Vector3 offset = new Vector3(0f, 0f, -5f);
    public float smoothSpeed = 5f;


    void Update()
    {
        //Locks the camera on the owner of the game only.
        //if (!IsOwner)
        //{
        //    return;
        //}

        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        
    }
}
