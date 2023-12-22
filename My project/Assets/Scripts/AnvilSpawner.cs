using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilSpawner : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float conveyorSpeed;
    [SerializeField] private float minDistanceBetween;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform maxPosition;

    private List<GameObject> anvils = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }
    IEnumerator SpawnCoroutine()
    {
        if(anvils.Count == 0 || spawnPoint.position.z < anvils[0].transform.position.z + minDistanceBetween){
            GameObject anvil = Instantiate(prefab);
            anvil.GetComponent<Item>().onPickup += AnvilPickedUp;


            prefab.transform.position = spawnPoint.position;
            anvils.Add(anvil);
        }        
        yield return new WaitForSeconds(Random.Range(spawnCooldown, spawnCooldown * 2));
        StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        for(int i = 0; i < anvils.Count; i++){
            if (anvils[i] == null)
            {
                anvils.RemoveAt(i);
                continue;
            }
            Vector3 nextPosition = maxPosition.position;
            if (i > 0)
                nextPosition = anvils[i - 1].transform.position;
            if(anvils[i].transform.position.z + minDistanceBetween < nextPosition.z)
                anvils[i].transform.position -= conveyorSpeed * Time.deltaTime * transform.right;
        }
    }

    void AnvilPickedUp(Item item)
    {
        anvils.Remove(item.gameObject);
    }
}
