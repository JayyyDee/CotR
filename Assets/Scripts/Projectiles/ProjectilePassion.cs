using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePassion : NetworkBehaviour
{
        public float deathTimer;
        private void Start()
        {
            Destroy(gameObject, deathTimer);
        StartCoroutine(Explosion());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {

                Destroy(gameObject);
                Debug.Log("HIT");
                collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(200);

                //Damage
            }


        }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        transform.localScale = new Vector3(2, 2, 2);
    }
    
}
