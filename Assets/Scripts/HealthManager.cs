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
    }



    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        TakeDamageClientRpc(damage);

      
    }

    [ClientRpc]
    private void TakeDamageClientRpc(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
            Debug.Log(currentHealth);
        }

        if (currentHealth <= 0) {
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
}
