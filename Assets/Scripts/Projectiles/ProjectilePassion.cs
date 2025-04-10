using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class ProjectilePassion : NetworkBehaviour
{
        public float deathTimer;

        public GameObject explosionPrefab;
        private int damage = 50;
        private bool boost = false;

    public float fireForce;
        private void Start()
        {
            StartCoroutine(Despawn(deathTimer));
            StartCoroutine(Explosion());
        }

    private void OnTriggerEnter2D(Collider2D collider)
        {

            if (collider.CompareTag("Enemy"))
            {

                StartCoroutine(Despawn(0)); 
                Debug.Log("HIT");
                collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);

                //Damage
            }
            if(collider.CompareTag("Walls")){
                StartCoroutine(Despawn(0));
        }


        }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        GetComponent<Rigidbody2D>().AddForce(transform.right * 20, ForceMode2D.Impulse);
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
        ExplosionServerRpc();
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void ExplosionServerRpc()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        if (boost)
        {
            explosion.transform.localScale = new Vector3(3, 3, 1);
            explosion.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        
        explosion.GetComponent<NetworkObject>().Spawn();
        //ExplosionClientRpc();
    }

    [ClientRpc]
    private void ExplosionClientRpc()
    {
        

    }

    IEnumerator Despawn(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject, 0f);
    }

}
