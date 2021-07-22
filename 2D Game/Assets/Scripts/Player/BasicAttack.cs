using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public Transform AttackPoint;
    public GameObject SlashPrefab;

    public float AttackRate = 4.5f;

    private float nextTimeToAttack = 0f;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToAttack)
        {
            nextTimeToAttack = Time.time + 1f / AttackRate;
            Attack();
        }
    }

    void Attack()
    {

        GameObject SlashEffect = Instantiate(SlashPrefab, AttackPoint.position, AttackPoint.rotation);
    }
}