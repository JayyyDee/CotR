using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBravoure : MonoBehaviour
{
    private GameObject hitbox;
    private GameObject preHitbox;
    
   

    public void Shoot()
    {
        hitbox = gameObject.transform.Find("Rotation").Find("BravoureHitbox").gameObject;
        preHitbox = gameObject.transform.Find("Rotation").Find("BravourePreHitbox").gameObject;
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        preHitbox.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        hitbox.SetActive(true);
        preHitbox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
    }


}
