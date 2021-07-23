using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private GameObject SlashPrefab;

    [SerializeField] private float AttackRate = 4.5f;

    private float nextTimeToAttack = 0f;

    private bool attackPressed;
    private bool attackInputUsed;

    private void Start()
    {

    }

    private void Update()
    {
        if (attackPressed && !attackInputUsed && Time.time >= nextTimeToAttack)
        {
            attackInputUsed = true;
            nextTimeToAttack = Time.time + 1f / AttackRate;
            Attack();
        }
    }

    private void Attack()
    {

        GameObject SlashEffect = Instantiate(SlashPrefab, AttackPoint.position, AttackPoint.rotation);
    }

    private void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
        if (attackPressed)
        {
            attackInputUsed = false;
        }
    }
}