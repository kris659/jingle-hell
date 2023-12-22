using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class SantaController : MonoBehaviour, ITakingDamage
{
    [SerializeField] private GameObject player;
    [SerializeField] private int maxHealth;
    [SerializeField] private float health;

    [SerializeField] private float attacksCooldown;
    [SerializeField] private AnimationCurve santaAttackSpeedMultByHP;


    [SerializeField] private GameObject presentPrefab;
    [SerializeField] private GameObject bigPresentPrefab;
    [SerializeField] private Transform bigPresentSpawnPoint;

    [SerializeField] private float throwingForce;

    [SerializeField] private Transform minPresentSpawnPosition;
    [SerializeField] private Transform maxPresentSpawnPosition;


    [SerializeField] private float santaDistance;


    private NavMeshAgent navMeshAgent;

    private bool goingToPlayer = true;

    private void Awake()
    {
        health = maxHealth;
        StartCoroutine(SelectAction());
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

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

    IEnumerator SelectAction()
    {
        yield return new WaitForSeconds(attacksCooldown / santaAttackSpeedMultByHP.Evaluate(health / maxHealth));
        StartCoroutine(SpawnExplosivePresents());
        yield return new WaitForSeconds(attacksCooldown / santaAttackSpeedMultByHP.Evaluate(health / maxHealth));
        StartCoroutine(ThrowPresent());
        StartCoroutine(SelectAction());
    }

    IEnumerator GoToPlayer(){
        goingToPlayer = true;
        navMeshAgent.SetDestination(player.transform.position);
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(() => Vector3.Distance(transform.position, player.transform.position) <= santaDistance);
        
        if(Vector3.Distance(transform.position, player.transform.position) <= santaDistance){
            StartCoroutine(SelectAction());
            goingToPlayer = false;
        }
        else
            StartCoroutine(GoToPlayer());
    }

    IEnumerator Stomp()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator SpawnExplosivePresents(){
        int howMuch = Random.Range(3, 6);
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
    IEnumerator ThrowPresent()
    {        
        GameObject present = Instantiate(bigPresentPrefab, transform);
        present.transform.position = bigPresentSpawnPoint.position;
        Rigidbody rb = present.GetComponent<Rigidbody>();
        rb.useGravity = false;
        
        yield return new WaitForSeconds(1);
        if (rb != null)
        {
            rb.useGravity = false;
            present.transform.parent = null;
            Vector3 forceDirection = player.transform.position - present.transform.position;
            float power = throwingForce;
            rb.AddForce(forceDirection * power, ForceMode.Impulse);
            Destroy(present, 3);
        }
    }

    Vector3 GetPresentSpawnPosition()
    {
        Vector3 position = new Vector3(
            Random.Range(minPresentSpawnPosition.position.x, maxPresentSpawnPosition.position.x),
            Random.Range(minPresentSpawnPosition.position.y, maxPresentSpawnPosition.position.y),
            Random.Range(minPresentSpawnPosition.position.z, maxPresentSpawnPosition.position.z));
        return position;
    }    
}
