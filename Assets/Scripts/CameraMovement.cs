using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : NetworkBehaviour
{
    [SerializeField] public Transform player;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothSpeed = 5f;

    void AttachCamera()
    {
        if (player == null)
        {
            // Try to find the target dynamically if it appears later
            GameObject foundObject = GameObject.FindWithTag("Player");
            if (foundObject != null)
            {
                player = foundObject.transform;
            }
        }

        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
