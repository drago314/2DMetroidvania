using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int basicAttackDamage;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float sideAttackKnockback;
    [SerializeField] private float upAttackKnockback;
    [SerializeField] private float downAttackKnockback;
    [SerializeField] private float sideAttackRadius;
    [SerializeField] private float upAttackRadius;
    [SerializeField] private float downAttackRadius;
    [SerializeField] private SideAttackToggle sideAttack;
    [SerializeField] private UpAttackToggle upAttack;
    [SerializeField] private UpAttackToggle downAttack;

    [SerializeField] private int animeDashDamage;
    [SerializeField] private float animeDashForce;
    [SerializeField] private float animeDashRange;
    [SerializeField] private float animeDashInvincibilityTime;
    [SerializeField] private float animeDashTime;
    [SerializeField] private float animeDashCooldown;
    [SerializeField] private float maxTargetDegreeError;

    private PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private Rigidbody2D body;
    private LayerMask enemyLayer;
    private LayerMask groundLayer;
    private LayerMask playerLayer;
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
    private Enemy animeDashTarget;
    private Vector2 animeDashDirection;

    private void Start()
    {
        playerActions = gameObject.GetComponent<PlayerActions>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();

        body = playerActions.body;
        defaultGravity = playerActions.defaultGravity;
        enemyLayer = playerActions.GetEnemyLayer();
        groundLayer = playerActions.GetGroundLayer();
        playerLayer = playerActions.GetPlayerLayer();
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
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
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
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
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
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
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
        if (animeDashing && !reachedTarget && Vector2.Distance(body.position, animeDashTarget.transform.position) < 0.2f)
        {
            Invoke("RemoveInvincibility", animeDashInvincibilityTime);
            animeDashTarget.GetComponent<Health>().Damage(new Damage(animeDashDamage, gameObject, Damage.PLAYER_ANIME_DASH));

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
                Vector2 player = transform.position;
                float joystickDegree = Trigonometry.RadianFromPosition(playerActions.leftJoystick.x, playerActions.leftJoystick.y);
                joystickDegree = joystickDegree * 180 / Mathf.PI;
                float targetDegreeError = 360;
                float targetDistance = animeDashRange + 1;

                List<Enemy> enemyTargets = new List<Enemy>();
                foreach (Collider2D possibleTarget in possibleTargets)
                {
                    if (possibleTarget.GetComponent<Enemy>() != null && !possibleTarget.GetComponent<Enemy>().IsShadow())
                        enemyTargets.Add(possibleTarget.GetComponent<Enemy>());
                }

                foreach (Enemy possibleTarget in enemyTargets)
                {
                    Vector2 enemy = possibleTarget.transform.position;
                    float enemyRadian = Trigonometry.RadianFromPosition(enemy.x - player.x, enemy.y - player.y);
                    float enemyDegreeError = Mathf.Abs((enemyRadian * 180 / Mathf.PI) - (joystickDegree));
                    if (enemyDegreeError > 180)
                        enemyDegreeError = (360 - enemyDegreeError);

                    float enemyDistance = Vector2.Distance(player, enemy);

                    if (enemyDegreeError <= maxTargetDegreeError / 2 && enemyDistance < targetDistance)
                    {
                        animeDashTarget = possibleTarget;
                        targetDegreeError = enemyDegreeError;
                        targetDistance = enemyDistance;
                    }
                    else if(targetDegreeError > maxTargetDegreeError && enemyDegreeError < targetDegreeError)
                    {
                        animeDashTarget = possibleTarget;
                        targetDegreeError = enemyDegreeError;
                        targetDistance = enemyDistance;
                    }
                }

                if (enemyTargets.Count != 0)
                {
                    Vector2 target = animeDashTarget.transform.position;
                    Vector2 direction = target - player;
                    direction.Normalize();
                    animeDashDirection = direction * animeDashForce;

                    RaycastHit2D raycastHit = Physics2D.Raycast(player, target, Vector2.Distance(player, target), groundLayer);

                    if (!raycastHit)
                    {
                        playerActions.iFrame.Invincible(true);
                        Physics2D.IgnoreLayerCollision(10, 8, true);

                        animeDashTarget.hitsUntilShadow -= 1;

                        body.gravityScale = 0;
                        animeDashing = true;
                        reachedTarget = false;

                        body.velocity = animeDashDirection;
                    }
                }
            }
        }
    }

    private void RemoveInvincibility()
    {
        playerActions.iFrame.Invincible(false);
        Physics2D.IgnoreLayerCollision(10, 8, false);
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
