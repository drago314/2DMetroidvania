using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float sideAttackKnockback;
    [SerializeField] private float upAttackKnockback;
    [SerializeField] private float downAttackKnockback;
    [SerializeField] private float sideAttackRadius;
    [SerializeField] private float upAttackRadius;
    [SerializeField] private float downAttackRadius;
    [SerializeField] private SideAttackToggle sideAttack;
    [SerializeField] private UpAttackToggle upAttack;
    [SerializeField] private UpAttackToggle downAttack;

    [SerializeField] private float animeDashForce;
    [SerializeField] private float animeDashRange;
    [SerializeField] private float animeDashInvincibilityTime;
    [SerializeField] private float animeDashTime;
    [SerializeField] private float animeDashCooldown;

    private PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private Rigidbody2D body;
    private LayerMask enemyLayer;
    private float defaultGravity;

    private bool attackPressed;
    private bool attackInputUsed;
    private float attackTimer = 0f;
    private float attackCooldownTimer = 0f;

    private bool animeDashPressed;
    private bool animeDashInputUsed;
    private bool animeDashing = false;
    private bool reachedTarget;
    private float animeDashTimer = 0f;
    private float animeDashCooldownTimer = 0f;
    private Vector2 animeDashTarget;
    private Vector2 animeDashDirection;

    private void Start()
    {
        playerActions = gameObject.GetComponent<PlayerActions>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        body = playerActions.body;
        defaultGravity = playerActions.defaultGravity;
        enemyLayer = playerActions.GetEnemyLayer();
    }

    public void Attack()
    {
        BasicAttack();
        AnimeDash();
    }

    public bool CheckControl()
    {
        bool hasControl = false;

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else if (animeDashing)
        {
            AnimeDash();
            if (animeDashTimer > 0)
                animeDashTimer -= Time.deltaTime;
        }
        else
        { 
            if (attackCooldownTimer > 0)
                attackCooldownTimer -= Time.deltaTime;

            if (animeDashCooldownTimer > 0)
                animeDashCooldownTimer -= Time.deltaTime;

            hasControl = true;
        }

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

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(damage, gameObject, Damage.PLAYER));
                    enemy.GetComponent<EnemyKnockback>().Knockback(Vector2.up);
                    hit = true;
                }

                if (hit)
                {
                    body.velocity = new Vector2(body.velocity.x, -upAttackKnockback);
                }

                Invoke("EndUpAttack", attackTime);
            }
            else if (playerActions.leftJoystick.y < -0.2 && !playerActions.isGrounded)
            {
                downAttack.Attack();

                Collider2D[] enemies = Physics2D.OverlapCircleAll(downAttack.transform.position, downAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(damage, gameObject, Damage.PLAYER));
                    enemy.GetComponent<EnemyKnockback>().Knockback(Vector2.down);
                    hit = true;
                }

                if (hit)
                {
                    body.velocity = new Vector2(body.velocity.x, downAttackKnockback);
                }

                Invoke("EndDownAttack", attackTime);
            }
            else
            {
                sideAttack.Attack(playerActions.facingRight);

                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, sideAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(damage, gameObject, Damage.PLAYER));
                    if (playerActions.facingRight)
                        enemy.GetComponent<EnemyKnockback>().Knockback(Vector2.right);
                    else
                        enemy.GetComponent<EnemyKnockback>().Knockback(Vector2.left);
                    hit = true;
                }

                if(hit)
                {
                    if (playerActions.facingRight)
                        body.velocity = new Vector2(-sideAttackKnockback, body.velocity.y);
                    else
                        body.velocity = new Vector2(sideAttackKnockback, body.velocity.y);
                    attackTimer = attackTime;
                }

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

    private void AnimeDash()
    {
        if (animeDashing && !reachedTarget && Vector2.Distance(body.position, animeDashTarget) < 0.2f)
        {
            Invoke("RemoveInvincibility", animeDashInvincibilityTime);
            reachedTarget = true;
            animeDashCooldownTimer = animeDashCooldown;
            animeDashTimer = animeDashTime;
            body.velocity = animeDashDirection;
        }
        else if (animeDashing && reachedTarget && animeDashTimer <= 0)
        {
            body.gravityScale = defaultGravity;
            body.velocity = Vector2.zero;
            playerMovement.AirResetMovement();
            animeDashing = false;
        }
        else if (animeDashPressed && !animeDashInputUsed && animeDashCooldownTimer <= 0)
        {
            animeDashInputUsed = true;
            Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(body.position, animeDashRange, enemyLayer);
            if (possibleTargets.Length > 0)
            {
                playerActions.iFrame.Invincible(true);

                body.gravityScale = 0;
                animeDashing = true;
                reachedTarget = false;

                animeDashTarget = possibleTargets[0].transform.position;
                DashToPosition(animeDashTarget);
            }
        }
    }

    private void DashToPosition(Vector2 target)
    {
        Vector2 player = transform.position; ;
        Vector2 direction = target - player;
        direction.Normalize();
        animeDashDirection = direction * animeDashForce;
        body.velocity = animeDashDirection;
    }

    private void RemoveInvincibility()
    {
        playerActions.iFrame.Invincible(false);
    }

    private void OnAttack(InputValue value)
    {
        attackPressed = value.isPressed;
        if (attackPressed)
        {
            attackInputUsed = false;
        }
    }

    private void OnAnimeDash(InputValue value)
    {
        animeDashPressed = value.isPressed;
        if (animeDashPressed)
        {
            animeDashInputUsed = false;
        }
    }
}
