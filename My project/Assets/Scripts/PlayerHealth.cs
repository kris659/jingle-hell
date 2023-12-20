using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ITakingDamage
{
    [SerializeField] private int maxHealth;
    [SerializeField] private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 5)
            return;
        
        Debug.Log("Player damaged " +  damage);
        
        health -= damage;
        health = Mathf.Max(health, 0);
        UIManager.playerHealthUI.UpdateUI(health / maxHealth);
        if (health == 0)
            GameOver();
    }

    void GameOver()
    {
        Debug.Log("GameOver");
    }
}
