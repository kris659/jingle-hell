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
    [SerializeField] private float stompRadius;
    [SerializeField] private float stompPower;
    [SerializeField] private float stompDamageMult;
    [SerializeField] private int santaAttackChance;
    [SerializeField] private GameObject dustParticlesPrefab;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private Animator animator;

    private bool walking = false;
    private bool stomping = false;
    private bool bigPresent = false;
    private bool smallPresents = false;

    private bool immortal = true;
    private void Awake()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();        
    }

    private void Start()
    {
        BossEnter.OnBossEnter += () => { StartCoroutine(SelectAction());
            UIManager.santaHealthUI.UpdateUI(health / maxHealth); 
            immortal = false;
        };
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
        if (immortal)
            return;
        int index = Random.Range(0, 3);
        if (index == 0)
            AudioManager.PlaySound(AudioManager.Sound.SantaDamage1);
        if (index == 1)
            AudioManager.PlaySound(AudioManager.Sound.SantaDamage2);
        if (index == 2)
            AudioManager.PlaySound(AudioManager.Sound.SantaDamage3);


        health -= damage;
        health = Mathf.Max(health, 0);
        UIManager.santaHealthUI.UpdateUI(health / maxHealth);
        if (health == 0)
            KilledSanta();
    }

    private void KilledSanta()
    {
        Debug.Log("KilledSanta");
        UIManager.gameOverUI.OpenUI("Victory!");
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
        AudioManager.PlaySound(AudioManager.Sound.StompAttack);
        stomping = true;
        animator.SetBool("stomping", stomping);
        yield return new WaitForSeconds(1.45f);
        Destroy(Instantiate(dustParticlesPrefab, transform), 1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, stompRadius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out PlayerHealth playerHealth))
                playerHealth.TakeDamage(stompDamageMult * (stompRadius - Vector3.Distance(transform.position, playerHealth.transform.position)));
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.TryGetComponent(out Rigidbody rb))
            {
                Vector3 forceDirection = rb.transform.position - transform.position;
                //float power = stompRadius * (stompRadius - Vector3.Distance(transform.position, rb.transform.position));
                //if (rb.GetComponent<PlayerHealth>() != null)
                //    forceDirection.x *= 5;
                //rb.AddForce(forceDirection * power, ForceMode.Impulse);
                if(rb.GetComponent<PlayerHealth>() != null)
                    rb.AddExplosionForce(stompPower * 40, transform.position, stompRadius);
                else
                    rb.AddExplosionForce(stompPower, transform.position, stompRadius);
            }
        }


        yield return new WaitForSeconds(0.55f);
        stomping = false;
        animator.SetBool("stomping", stomping);
    }

    IEnumerator SpawnExplosivePresents(){
        AudioManager.PlaySound(AudioManager.Sound.SmallPresentsAttack);
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
            presents[i].AddComponent<Rigidbody>();
        }
        smallPresents = false;
        animator.SetBool("smallPresents", smallPresents);
    }
    IEnumerator ThrowPresent()
    {
        AudioManager.PlaySound(AudioManager.Sound.BigPresentAttack);
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
            Destroy(present, 5);
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, stompRadius);
    }
}
