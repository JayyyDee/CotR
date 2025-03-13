using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HealthManager : NetworkBehaviour
{
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

    [ServerRpc]
    private void TakeDamageServerRpc(int damage)
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
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Invoke("Death", 2);

        }
    }
    [ServerRpc]
    private void HealingServerRpc(int heal)
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
    }
}
