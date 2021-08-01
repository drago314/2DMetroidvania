using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float attackRadius;
    [SerializeField] private SideAttackToggle sideAttack;

    private PlayerActions playerActions;
    private LayerMask enemyLayer;

    private float attackTimer = 0f;

    private bool attackPressed;
    private bool attackInputUsed;

    private void Start()
    {
        playerActions = GetComponent<PlayerActions>();

        enemyLayer = playerActions.GetEnemyLayer();
    }

    public void Attack()
    {
        BasicAttack();
    }

    public bool CheckControl()
    {
        bool hasControl = false;

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
        else
            hasControl = true;

        return hasControl;
    }

    private void BasicAttack()
    {
        if (attackPressed && !attackInputUsed)
        {
            attackInputUsed = true;
            sideAttack.Attack();
            attackTimer = attackTime;

            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

            foreach (Collider2D enemy in enemies)
                enemy.GetComponent<Health>().Damage(damage);

            Invoke("EndAttack", attackTime);
        }
    }

    private void EndAttack()
    {
        sideAttack.EndAttack();
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
