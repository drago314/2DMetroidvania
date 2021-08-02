using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float sideAttackRadius;
    [SerializeField] private float upAttackRadius;
    [SerializeField] private float downAttackRadius;
    [SerializeField] private SideAttackToggle sideAttack;
    [SerializeField] private UpAttackToggle upAttack;
    [SerializeField] private UpAttackToggle downAttack;

    private PlayerActions playerActions;
    private LayerMask enemyLayer;

    private float attackCooldownTimer = 0f;

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

        if (attackCooldownTimer > 0)
                attackCooldownTimer -= Time.deltaTime;

        hasControl = true;

        return hasControl;
    }

    private void BasicAttack()
    {
        if (attackPressed && !attackInputUsed && attackCooldownTimer <= 0)
        {
            attackInputUsed = true;

            if (playerActions.leftJoystick.y > 0.2)
            {
                upAttack.Attack();

                Collider2D[] enemies = Physics2D.OverlapCircleAll(upAttack.transform.position, upAttackRadius, enemyLayer);

                foreach (Collider2D enemy in enemies)
                    enemy.GetComponent<Health>().Damage(damage);

                Invoke("EndUpAttack", attackTime);
            }
            else if (playerActions.leftJoystick.y < -0.2 && !playerActions.isGrounded)
            {
                downAttack.Attack();

                Collider2D[] enemies = Physics2D.OverlapCircleAll(downAttack.transform.position, downAttackRadius, enemyLayer);

                foreach (Collider2D enemy in enemies)
                    enemy.GetComponent<Health>().Damage(damage);

                Invoke("EndDownAttack", attackTime);
            }
            else
            {
                sideAttack.Attack(playerActions.facingRight);

                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, sideAttackRadius, enemyLayer);

                foreach (Collider2D enemy in enemies)
                    enemy.GetComponent<Health>().Damage(damage);

                Invoke("EndSideAttack", attackTime);
            }
        }
    }

    private void EndSideAttack()
    {
        sideAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
    }

    private void EndUpAttack()
    {
        upAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
    }

    private void EndDownAttack()
    {
        downAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
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
