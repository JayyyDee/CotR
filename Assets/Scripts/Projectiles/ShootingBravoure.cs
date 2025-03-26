using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBravoure : MonoBehaviour
{
    public GameObject hitbox;
    public GameObject preHitbox;
    
   

    public void Shoot()
    { 
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        preHitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitbox.SetActive(true);
        preHitbox.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
    }


}
