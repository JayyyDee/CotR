using System.Collections;
using System.Collections.Generic;
using UnityEngine;

void Start()
{
    Vector3[] positionArray = new Vector3[6];

    positionArray[0] = new Vector3(-19f, -1f, 0f);
    positionArray[1] = new Vector3(18f, 21f, 0f);
    positionArray[2] = new Vector3(18f, -21f, 0f);
    positionArray[3] = new Vector3(-10f, 21f, 0f);
    positionArray[4] = new Vector3(26f, -1f, 0f);
    positionArray[5] = new Vector3(-10f, -21f, 0f);

    //Player.transform.position = positionArray[Random.Range(0, positionArray.Length)];
}

//public static void Shuffle<T>