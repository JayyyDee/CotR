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
            Destroy(gameObject, deathTimer);
            StartCoroutine(Explosion());
        }

    private void OnTriggerEnter2D(Collider2D collider)
        {

            if (collider.CompareTag("Enemy"))
            {

                Destroy(gameObject);
                Debug.Log("HIT");
                collider.gameObject.GetComponent<HealthManager>().TakeDamageServerRpc(damage);

                //Damage
            }
            if(collider.CompareTag("Walls")){
                Destroy(gameObject);
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
        ExplosionClientRpc();
    }

    [ClientRpc]
    private void ExplosionClientRpc()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        //if (boost)
        //{
        //    GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        //    explosion.transform.localScale = new Vector3(3, 3, 1);
        //    explosion.GetComponent<SpriteRenderer>().color = Color.yellow;
        //}
        //else
        //{

        //    GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        //}
        
    }

}
