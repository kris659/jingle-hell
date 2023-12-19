using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Catapult : MonoBehaviour
{
    public GameObject projectile;
    public Transform catapultTr;
    public Transform spawnPoint;

    [SerializeField] public float shootForce;
    [SerializeField] public float shootDuration;

    private Animator animator;
    private bool isShooting = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //catapultTr = transform.GetChild(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Shoot();
    }

    private void Shoot()
    {
        if (isShooting)
            return;
        Debug.Log(catapultTr.name);
        isShooting = true;
        catapultTr.DOLocalRotate(new Vector3(0, 0, -90), shootDuration).SetEase(Ease.InCubic).OnComplete(() => {
            catapultTr.DOLocalRotate(new Vector3(0, 0, 0), 2 * shootDuration).SetEase(Ease.OutSine).OnComplete(() => { 
                isShooting = false;
            });
        
        });
        StartCoroutine(SpawnCube());
    }

    IEnumerator SpawnCube()
    {
        GameObject cube = Instantiate(projectile);
        cube.transform.position = spawnPoint.position + cube.transform.localScale.y * 0.5f * spawnPoint.up;
        cube.transform.parent = spawnPoint;
        yield return new WaitForSeconds(shootDuration * 0.85f);
        cube.transform.parent = null;
        cube.GetComponent<BoxCollider>().size = new Vector3(1, 0.5f, 1);
        cube.AddComponent<Rigidbody>().AddForce((spawnPoint.up - spawnPoint.right * 0.2f) * shootForce, ForceMode.Impulse);
        yield return new WaitForSeconds(shootDuration * 0.15f);
        cube.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
        cube.transform.parent = null;
    }

}
