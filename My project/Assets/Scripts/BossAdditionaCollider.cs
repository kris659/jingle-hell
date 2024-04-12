using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAdditionaCollider : MonoBehaviour
{
    [SerializeField] private GameObject colliderGameObject;
    void Start()
    {
        BossEnter.OnBossEnter += () => { colliderGameObject.SetActive(true); };
    }

    // U
}
