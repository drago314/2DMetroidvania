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
    [SerializeField] private AttackToggle rightAttack;
    [SerializeField] private AttackToggle leftAttack;
    [SerializeField] private AttackToggle upAttack;
    [SerializeField] private AttackToggle downAttack;

    [SerializeField] private int animeDashDamage;
    [SerializeField] private float animeDashForce;
    [SerializeField] private float animeDashRange;
    [SerializeField] private float animeDashInvincibilityTime;
    [SerializeField] private float animeDashTime;
    [SerializeField] private float animeDashCooldown;
    [SerializeField] private float maxTargetDegreeError;
    [SerializeField] private GameObject animeDashPointerPrefab;

    [SerializeField] private float daggerCooldownTime;
    [SerializeField] private int daggerDamage;
    [SerializeField] private float daggerSpeed;
    [SerializeField] private GameObject dagger;

    private PlayerActions playerActions;
    private PlayerMovement playerMovement;
    private Rigidbody2D body;
    private LayerMask enemyLayer;
    private LayerMask groundLayer;
    private LayerMask playerLayer;
    private float defaultGravity;

    public bool rightAttacking { get; private set; } = false;
    public bool leftAttacking { get; private set; } = false;
    public bool upAttacking { get; private set; } = false;
    public bool downAttacking { get; private set; } = false;
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
    private GameObject currentPointer;

    private bool launchDaggerPressed;
    private bool launchDaggerInputUsed;
    private float daggerCooldownTimer;

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
        LaunchDagger();
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

            if (daggerCooldownTimer > 0)
                daggerCooldownTimer -= Time.deltaTime;

            hasControl = true;
        }

        return hasControl;
    }

    public void CheckAttack()
    {
        FindAnimeDashTarget();
    }

    private void BasicAttack()
    {
        if (attackPressed && !attackInputUsed && attackCooldownTimer <= 0)
        {
            attackInputUsed = true;

            if (playerActions.leftJoystick.y > 0.2)
            {
                upAttack.Attack();
                upAttacking = true;

                Collider2D[] enemies = Physics2D.OverlapCircleAll(upAttack.transform.position, upAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
                    hit = true;
                }

                if (hit)
                {
                    attackTimer = attackTime;
                    body.velocity = new Vector2(body.velocity.x, -upAttackKnockback);
                }

                Invoke("EndUpAttack", attackTime);
            }
            else if (playerActions.leftJoystick.y < -0.2 && !playerActions.isGrounded)
            {
                downAttack.Attack();
                downAttacking = true;

                Collider2D[] enemies = Physics2D.OverlapCircleAll(downAttack.transform.position, downAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
                    hit = true;
                }

                if (hit)
                {
                    attackTimer = attackTime;
                    body.velocity = new Vector2(body.velocity.x, downAttackKnockback);
                }

                Invoke("EndDownAttack", attackTime);
            }
            else if (playerActions.facingRight)
            {
                rightAttack.Attack();
                rightAttacking = true;

                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, sideAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
                    hit = true;
                }

                if(hit)
                {
                    attackTimer = attackTime;
                    body.velocity = new Vector2(-sideAttackKnockback, body.velocity.y);
                }

                Invoke("EndRightAttack", attackTime);
            }
            else
            {
                leftAttack.Attack();
                leftAttacking = true;

                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, sideAttackRadius, enemyLayer);

                bool hit = false;
                foreach (Collider2D enemy in enemies)
                {
                    enemy.GetComponent<Health>().Damage(new Damage(basicAttackDamage, gameObject, Damage.PLAYER_BASIC_ATTACK));
                    hit = true;
                }

                if (hit)
                {
                    attackTimer = attackTime;
                    body.velocity = new Vector2(sideAttackKnockback, body.velocity.y);
                }

                Invoke("EndLeftAttack", attackTime);
            }
        }
    }

    private void EndRightAttack()
    {
        rightAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
        rightAttacking = false;
    }

    private void EndLeftAttack()
    {
        leftAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
        leftAttacking = false;
    }

    private void EndUpAttack()
    {
        upAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
        upAttacking = false;
    }

    private void EndDownAttack()
    {
        downAttack.EndAttack();
        attackCooldownTimer = attackCooldown;
        downAttacking = false;
    }

    private void FindAnimeDashTarget()
    {
        if (!animeDashing)
        {
            Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(body.position, animeDashRange, enemyLayer);

            List<Enemy> enemyTargets = new List<Enemy>();
            foreach (Collider2D possibleTarget in possibleTargets)
            {
                if (possibleTarget.GetComponent<Enemy>() != null && !possibleTarget.GetComponent<Enemy>().IsShadow())
                {
                    enemyTargets.Add(possibleTarget.GetComponent<Enemy>());
                }
            }

            Vector2 player = transform.position;
            float targetDegreeError = 360;
            float targetDistance = animeDashRange + 1;

            float joystickDegree = Trigonometry.RadianFromPosition(playerActions.leftJoystick.x, playerActions.leftJoystick.y);
            joystickDegree = joystickDegree * 180 / Mathf.PI;
            if (playerActions.leftJoystick == Vector2.zero)
                joystickDegree = Trigonometry.RadianToDegree(Trigonometry.RadianFromPosition(playerActions.lastLeftJoystick));

            Enemy possibleAnimeDashTarget = null;

            List<Enemy> viewableTargets = new List<Enemy>();
            foreach (Enemy possibleTarget in enemyTargets)
            {
                Vector2 target = possibleTarget.transform.position;
                RaycastHit2D raycastHit = Physics2D.Raycast(player, target - player, Vector2.Distance(player, target) + 2, groundLayer);
                if (!raycastHit)
                    viewableTargets.Add(possibleTarget);
            }

            foreach (Enemy possibleTarget in viewableTargets)
            {
                Vector2 enemy = possibleTarget.transform.position;
                float enemyRadian = Trigonometry.RadianFromPosition(enemy.x - player.x, enemy.y - player.y);
                float enemyDegreeError = Mathf.Abs((enemyRadian * 180 / Mathf.PI) - (joystickDegree));
                if (enemyDegreeError > 180)
                    enemyDegreeError = (360 - enemyDegreeError);

                float enemyDistance = Vector2.Distance(player, enemy);

                if (enemyDegreeError <= maxTargetDegreeError / 2 && enemyDistance < targetDistance)
                {
                    possibleAnimeDashTarget = possibleTarget;
                    targetDegreeError = enemyDegreeError;
                    targetDistance = enemyDistance;
                }
                else if (targetDegreeError > maxTargetDegreeError && enemyDegreeError < targetDegreeError)
                {
                    possibleAnimeDashTarget = possibleTarget;
                    targetDegreeError = enemyDegreeError;
                    targetDistance = enemyDistance;
                }
                else if (possibleAnimeDashTarget == null)
                {
                    possibleAnimeDashTarget = possibleTarget;
                }
            }

            if (possibleAnimeDashTarget == null)
            {
                if (animeDashTarget != null)
                    Destroy(currentPointer);
                animeDashTarget = null;
            }
            else if (possibleAnimeDashTarget != animeDashTarget)
            {
                if (animeDashTarget != null)
                    Destroy(currentPointer);
                animeDashTarget = possibleAnimeDashTarget;
                currentPointer = Instantiate(animeDashPointerPrefab, animeDashTarget.transform);
            }
        }
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
            Physics2D.IgnoreLayerCollision(10, 8, false);
        }
        else if (animeDashing && !reachedTarget)
        {
            playerActions.iFrame.Invincible(true);
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

            if(animeDashTarget != null)
            {
                Vector2 player = transform.position;
                Vector2 target = animeDashTarget.transform.position;
                Vector2 direction = target - player;
                direction.Normalize();
                animeDashDirection = direction * animeDashForce;

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

    private void RemoveInvincibility()
    {
        playerActions.iFrame.Invincible(false);
    }

    private void LaunchDagger()
    {
        if (launchDaggerPressed && !launchDaggerInputUsed && daggerCooldownTimer <= 0)
        {
            launchDaggerInputUsed = true;
            if (playerActions.facingRight)
            {
                LaunchDagger dagger = Instantiate(this.dagger, gameObject.transform.position, Quaternion.identity)
                        .GetComponent<LaunchDagger>();
                dagger.SetFlyDirection(new Vector2(daggerSpeed, 0));
            }
            else
            {
                LaunchDagger dagger = Instantiate(this.dagger, gameObject.transform.position, Quaternion.identity)
                        .GetComponent<LaunchDagger>();
                dagger.SetFlyDirection(new Vector2(-daggerSpeed, 0));
            }
            daggerCooldownTimer = daggerCooldownTime;
        }
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

    private void OnLaunchDagger(InputValue value)
    {
        launchDaggerPressed = value.isPressed;
        if (launchDaggerPressed)
        {
            launchDaggerInputUsed = false;
        }
    }

    private void OnGUI()
    {
        //GUI.Label(new Rect(1100, 10, 1000, 1000), "animeDashTarget: " + animeDashTarget.name);
    }
}
