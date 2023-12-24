using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElfController : MonoBehaviour, ITakingDamage
{
    public GameObject player;
    [SerializeField] private int maxHealth;
    [SerializeField] private float health;

    [SerializeField] private GameObject presentPrefab;
    [SerializeField] private Transform presentSpawnPoint;

    [SerializeField] private float throwingForce;



    [SerializeField] private float distance;
    [SerializeField] private int attackCooldown;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    private bool walking = false;
    private bool smallPresent = false;

    float startTime = 0;

    private void Awake()
    {
        health = maxHealth;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Init(Vector3 pos, GameObject player)
    {
        this.player = player;
        transform.DOMove(pos, 1).onComplete = () => {
            navMeshAgent.enabled = true;
            StartCoroutine(SelectAction());            
        };
    }

    private void Update()
    {
        if (smallPresent)
        {
            Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(target);
        }
    }

    public void TakeDamage(float damage)
    {
        int index = Random.Range(0, 3);
        if (index == 0)
            AudioManager.PlaySound(AudioManager.Sound.ElfDamage1);
        if (index == 1)
            AudioManager.PlaySound(AudioManager.Sound.ElfDamage2);
        if (index == 2)
            AudioManager.PlaySound(AudioManager.Sound.ElfDamage3);
        health = 0;
        //health -= damage;
        //health = Mathf.Max(health, 0);
        if (health == 0)
            KilledElf();
    }

    private void KilledElf()
    {
        Debug.Log("DeadElf");
        Destroy(gameObject);
    }

    IEnumerator SelectAction()
    {
        while (Vector3.Distance(transform.position, player.transform.position) >= distance)
        {
            if (!walking)
            {
                walking = true;
                animator.SetBool("walking", walking);                
            }
            navMeshAgent.SetDestination(player.transform.position);
            yield return new WaitForSeconds(0.2f);
        }
        navMeshAgent.SetDestination(transform.position);
        walking = false;
        animator.SetBool("walking", walking);
        yield return new WaitForSeconds(0.5f);
        while (Vector3.Distance(transform.position, player.transform.position) < distance)
        {   
            if(startTime + attackCooldown < Time.time){
                StartCoroutine(ThrowPresent());
                startTime = Time.time;
            }                        
            yield return new WaitUntil(() => !smallPresent && startTime + attackCooldown < Time.time);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(SelectAction());
    } 

    
    IEnumerator ThrowPresent()
    {
        AudioManager.PlaySound(AudioManager.Sound.BigPresentAttack);
        smallPresent = true;
        animator.SetBool("smallPresent", smallPresent);

        GameObject present = Instantiate(presentPrefab, transform);
        present.transform.position = presentSpawnPoint.position;
        Rigidbody rb = present.GetComponent<Rigidbody>();
        rb.useGravity = false;

        yield return new WaitForSeconds(1.25f);
        if (rb != null)
        {
            rb.useGravity = false;
            present.transform.parent = null;
            Vector3 forceDirection = (Vector3.up * 0.6f + player.transform.position - present.transform.position).normalized;
            float power = throwingForce;
            rb.AddForce(forceDirection * power, ForceMode.Impulse);
            Destroy(present, 5);
        }
        yield return new WaitForSeconds(1f);
        smallPresent = false;
        animator.SetBool("smallPresent", smallPresent);
    }
}
