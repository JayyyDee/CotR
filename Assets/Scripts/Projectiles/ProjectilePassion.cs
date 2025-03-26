using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePassion : NetworkBehaviour
{
        public float deathTimer;
        private int damage;
        private bool boost = false;
        private void Start()
        {
            Destroy(gameObject, deathTimer);
            StartCoroutine(Explosion());
        }

    private void Update()
    {
        Debug.Log(damage);
    }
    private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.collider.CompareTag("Enemy"))
            {

                Destroy(gameObject);
                Debug.Log("HIT");
                collision.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);

                //Damage
            }


        }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void SetBoost(bool boolean)
    {
        boost = boolean;
    }
    IEnumerator Explosion()
    {
        
        yield return new WaitForSeconds(0.2f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        if (boost)
        {
            transform.localScale = new Vector3(3, 3, 1);
        }
        else
        {
            
            transform.localScale = new Vector3(2, 2, 1);
        }
        
    }
    
}
