using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RingPatience : Ring
{
    public GameObject bulletPrefab;
    public Color bulletColor;
    private GameObject firePoint;
    private GameObject playerCharacter;
    private Vector3 mousePos;
    private Camera cam;

    
    public float range;
    private Queue<GameObject> zones= new Queue<GameObject>();



    public float fireForce = 1f;
    public float cooldown = 1f;
    private float timer;
    private bool canFire = true;
    private bool equiped = false;

    /*
    1 is normal speed
    <1 is slower
    >1 is faster
    */
    private float attackSpeed = 1f;


    // Update is called once per frame
    void Update()
    {
        //if (playerCharacter.GetComponent<NetworkObject>().IsOwner && equiped)
        //{
        //    
        //}
        if (equiped)
        {
            //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        //Vector3 aimDirection = mousePos - playerCharacter.transform.position;


        if (!canFire)
        {
            timer += (Time.deltaTime);
            if (timer > (cooldown / attackSpeed))
            {
                canFire = true;
                timer = 0;
            }
        }
        if (equiped && Input.GetMouseButton(0) && canFire)
        {
            Shoot();
        }
    }


    public override void Shoot()
    {
        Debug.Log("shoot");
        canFire = false;
        bulletPrefab.GetComponent<SpriteRenderer>().color = bulletColor;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, Quaternion.identity);
        bullet.GetComponent<ZonePatience>().player = playerCharacter;
        zones.Enqueue(bullet);
        if(zones.Count > 3)
        {
            Destroy(zones.Dequeue(),0);
        }

        
    }

    public override void Active()
    {
        //Activates/Explodes all zones 
    }

    public override void Passive()
    {
        playerCharacter.transform.Find("RangePatience").gameObject.SetActive(true);
    }

    public override void SetEquiped(bool boole)
    {
        equiped = boole;
    }
    public override bool GetCanFire()
    {
        return canFire;
    }

    public override void SetFirePoint(GameObject point)
    {
        firePoint = point;
    }

    public override void SetPlayer(GameObject player)
    {
        playerCharacter = player;
    }

    public override void SetAttackSpeed(float speed)
    {
        attackSpeed = speed;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DropServerRpc(Vector2 pos)
    {
        DropClientRpc(pos);
    }

    [ClientRpc]
    public void DropClientRpc(Vector2 pos)
    {
        gameObject.transform.position = pos;

        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<Ring>().SetEquiped(false);

    }


    public override void Drop()
    {
        playerCharacter.transform.Find("RangePatience").gameObject.SetActive(true);
        Vector2 pos = new Vector2(playerCharacter.transform.position.x + Random.Range(0, 2f), playerCharacter.transform.position.y + Random.Range(0, 2f));
        DropServerRpc(pos);
    }
    public void SetCamera(Camera camera)
    {
        cam = camera;
    }
}
