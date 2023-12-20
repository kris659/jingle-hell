using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SantaController : MonoBehaviour, ITakingDamage
{
    [SerializeField] private GameObject player;

    [SerializeField] private int maxHealth;
    [SerializeField] private float health;

    [SerializeField] private float attacksCooldown;
    [SerializeField] private AnimationCurve santaAttackSpeedMultByHP;


    [SerializeField] private GameObject presentPrefab;
    [SerializeField] private Vector3 minPresentSpawnPosition;
    [SerializeField] private Vector3 maxPresentSpawnPosition;

    private void Awake()
    {
        health = maxHealth;
        StartCoroutine(AttackCoroutine());
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        UIManager.santaHealthUI.UpdateUI(health / maxHealth);
        if (health == 0)
            KilledSanta();
    }

    private void KilledSanta()
    {
        Debug.Log("KilledSanta");
        Destroy(gameObject);
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attacksCooldown / santaAttackSpeedMultByHP.Evaluate(health / maxHealth));
        StartCoroutine(SpawnExplosivePresents());
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator SpawnExplosivePresents(){
        int howMuch = Random.Range(3, 8);
        List<Rigidbody> presents = new List<Rigidbody>();
        for(int i = 0; i < howMuch; i++){
            GameObject present = Instantiate(presentPrefab);
            present.transform.position = GetPresentSpawnPosition();
            Rigidbody rb = present.GetComponent<Rigidbody>();
            rb.useGravity = false;
            presents.Add(rb);
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < howMuch; i++)
        {
            if (presents[i] == null) 
                continue;
            presents[i].useGravity = true;
        }
    }

    Vector3 GetPresentSpawnPosition()
    {
        Vector3 position = new Vector3(
            Random.Range(minPresentSpawnPosition.x, maxPresentSpawnPosition.x),
            Random.Range(minPresentSpawnPosition.y, maxPresentSpawnPosition.y),
            Random.Range(minPresentSpawnPosition.z, maxPresentSpawnPosition.z));
        return position;
    }

    
}
