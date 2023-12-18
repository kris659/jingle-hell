using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnPoint;

    [SerializeField] public float shootForce;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Shoot();
    }

    private void Shoot()
    {
        animator.Play("Base Layer.CatapultShoot");
        StartCoroutine(SpawnCube());
    }

    IEnumerator SpawnCube()
    {
        yield return new WaitForSeconds(1.0f/3.0f);
        GameObject cube = Instantiate(projectile);
        cube.transform.position = spawnPoint.position + cube.transform.localScale.magnitude * 0.5f * spawnPoint.up;
        cube.transform.GetComponent<Rigidbody>().AddForce((spawnPoint.up - spawnPoint.right * 0.5f) * shootForce, ForceMode.Impulse);
    }
}
