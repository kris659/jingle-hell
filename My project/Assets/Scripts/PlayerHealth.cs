using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        UIManager.healthUI.UpdateUI((float)health / maxHealth);
        if (health == 0)
            GameOver();
    }

    void GameOver()
    {
        Debug.Log("GameOver");
    }
}
