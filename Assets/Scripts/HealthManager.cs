using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HealthManager : NetworkBehaviour
{
    [SerializeField] private GameObject deathScreen;
    public HealthBarManager healthBar;
    public int maxHealth = 1000;
    private int currentHealth;

    //Passive healing
    private float healCooldown = 0.05f;
    private float healTimer;

    public float hitCooldown = 5f;
    private float hitTimer;
    private bool isHit = false;

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {           
            TakeDamageServerRpc(100);
        }

        //To test for healing, press P
        if (Input.GetKeyDown(KeyCode.P))
        {
            HealingServerRpc(100);
        }

        PassiveHealingRecovery();

        //Passive heal overtime when not hit for 5 seconds
        if (isHit == false)
            healTimer += (Time.deltaTime);
            if (healTimer > healCooldown) {
                HealingServerRpc(5);
                healTimer = 0;
            }
     
    }


    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        TakeDamageClientRpc(damage);
        AkUnitySoundEngine.PostEvent("Play_SFX_Hit2__itemnumber", this.gameObject);



    }

    [ClientRpc]
    private void TakeDamageClientRpc(int damage)
    {
        if (currentHealth > 0)
        {
            //Reset passive healing
            isHit = true;
            hitTimer = 0;

            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            Debug.Log(currentHealth);
        }

        if (currentHealth <= 0) {
            AkUnitySoundEngine.PostEvent("Play_FULL_SFX_Death_Type1__itemnumber", this.gameObject);
            Death();
            
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void HealingServerRpc(int heal)
    {
        HealingClientRpc(heal);
    }

    [ClientRpc]
    void HealingClientRpc(int heal)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Death() {
        gameObject.GetComponent<Inventory>().Death();
        gameObject.GetComponent<GemManager>().Death();
        //gameObject.GetComponent<DeathScreenUI>().Death();
        gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(true);
    }

    private void PassiveHealingRecovery() {
        if (isHit == true) {
            hitTimer += (Time.deltaTime);
            if (hitTimer > hitCooldown) {
                isHit = false;
                hitTimer = 0;
            }
        }
    }
}
