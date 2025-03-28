using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HidePointer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
