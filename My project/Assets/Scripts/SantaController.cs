using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform[] smallPresentSpawnPoints;

    [SerializeField] private float throwingForce;

    [SerializeField] private Transform minPresentSpawnPosition;
    [SerializeField] private Transform maxPresentSpawnPosition;


    [SerializeField] private float santaDistance;
    [SerializeField] private int santaAttackChance;


    private NavMeshAgent navMeshAgent;

    [SerializeField] private Animator animator;

    private bool walking = false;
    private bool stomping = false;
    private bool bigPresent = false;
    private bool smallPresents = false;

    private void Awake()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();        
    }

    private void Start()
    {
        StartCoroutine(SelectAction());
    }

    private void Update()
    {
        if (bigPresent){
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(target);
        }
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
        while(Random.Range(0, 1000) >= santaAttackChance && Vector3.Distance(transform.position, player.transform.position) >= santaDistance)
        {
            if (!walking){
                animator.SetBool("walking", true);
                walking = true;
            }
            navMeshAgent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("walking", false);
        walking = false;
        if(Vector3.Distance(transform.position, player.transform.position) < santaDistance){
            int random = Random.Range(0, 2);
            if (random == 0)
                StartCoroutine(ThrowPresent());
            if (random == 1)
                StartCoroutine(Stomp());
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                StartCoroutine(ThrowPresent());
            if (random == 1)
                StartCoroutine(SpawnExplosivePresents());
        }
            

        yield return new WaitUntil(() => !bigPresent && !smallPresents && !stomping);
        yield return new WaitForSeconds(1f);
        StartCoroutine(SelectAction());
    }

    IEnumerator Stomp()
    {
        stomping = true;
        animator.SetBool("stomping", stomping);
        yield return new WaitForSeconds(2f);
        stomping = false;
        animator.SetBool("stomping", stomping);
    }

    IEnumerator SpawnExplosivePresents(){
        smallPresents = true;
        animator.SetBool("smallPresents", smallPresents);

        int howMuch = 4;// Random.Range(4,4);
        List<GameObject> presents = new List<GameObject>();
        for(int i = 0; i < howMuch; i++){
            GameObject present = Instantiate(presentPrefab);
            present.transform.position = smallPresentSpawnPoints[i].position;            
            
            Destroy(present.GetComponent<Rigidbody>());
            Collider collider = present.GetComponent<Collider>();
            collider.enabled = false;
            presents.Add(present);
        }
        yield return new WaitForSeconds(1);
        for (int i = 0; i < howMuch; i++)
        {
            if (presents[i] == null) 
                continue;
            presents[i].transform.DOMove(GetPresentSpawnPosition(), 1);
            
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < howMuch; i++)
        {
            if (presents[i] == null)
                continue;
            Collider collider = presents[i].GetComponent<Collider>();
            collider.enabled = true;
            presents[i].AddComponent<Rigidbody>();//.useGravity = true;
        }
        smallPresents = false;
        animator.SetBool("smallPresents", smallPresents);
    }
    IEnumerator ThrowPresent()
    {
        bigPresent = true;
        animator.SetBool("bigPresent", bigPresent);
        
        GameObject present = Instantiate(bigPresentPrefab, transform);
        present.transform.position = bigPresentSpawnPoint.position;
        Rigidbody rb = present.GetComponent<Rigidbody>();
        rb.useGravity = false;
        
        yield return new WaitForSeconds(1.25f);
        if (rb != null)
        {
            rb.useGravity = false;
            present.transform.parent = null;
            Vector3 forceDirection = (player.transform.position - present.transform.position).normalized;
            float power = throwingForce;
            rb.AddForce(forceDirection * power, ForceMode.Impulse);
            Destroy(present, 3);
        }
        yield return new WaitForSeconds(1f);
        bigPresent = false;
        animator.SetBool("bigPresent", bigPresent);        
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
