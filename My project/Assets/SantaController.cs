using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    [SerializeField] private float attacksCooldown;
    [SerializeField] private AnimationCurve santaAttackSpeedMultByHP;

    private void Awake()
    {
        health = maxHealth;
    }

    private void Start()
    {
        
    }

    IEnumerator AttacCoroutine()
    {
        yield return new WaitForSeconds(attacksCooldown / santaAttackSpeedMultByHP.Evaluate(health / maxHealth));
    }
}
