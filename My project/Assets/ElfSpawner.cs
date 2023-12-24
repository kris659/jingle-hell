using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElfSpawner : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject player;

    private void Start()
    {
        BossEnter.OnBossEnter += StartSpawn;
    }

    IEnumerator Spawn()
    {        
        GameObject elf = Instantiate(prefabs[Random.Range(0, prefabs.Length)]);
        elf.transform.position = spawnPoint.position;
        elf.GetComponent<NavMeshAgent>().enabled = false;
        elf.GetComponent<ElfController>().Init(startPoint.position, player);
        yield return new WaitForSeconds(Random.Range(cooldown, cooldown * 2));
        StartCoroutine(Spawn());
    }

    void StartSpawn()
    {
        StartCoroutine(Spawn());
    }
}
