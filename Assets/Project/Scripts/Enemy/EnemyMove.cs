using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        PATROL,
        MOVE,
        ATTACK,
        COOLDOWN
    }

    [Header("=== Target & Speed")]
    //public Transform player;
    public float moveSpeed = 1.0f;

    [Header("=== Ranges & Timers")]
    public float detectionRange = 5.0f;    // 索敵範囲
    public float attackRange = 1.2f;     // 攻撃可能距離
    public float idleTime = 2.0f;
    public float cooldownTime = 2.0f;
    [Header("=== Patrol Settings")]
    public float patrolStepDistance = 2.0f;
    [Header("=== Attack Settings")]
    public float attackForwardOffset = 1.5f;    // 攻撃時の距離
    public float attackMoveSpeed = 5.0f;         // 攻撃時の速度


    EnemyState currentState = EnemyState.IDLE;
    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Transform player;

    private float stateTimer = 0f;
    private bool patrolFlag = false;
    private Vector2 patrolTarget;

    // 攻撃用
    private Vector2 attackDirection;               // 攻撃時の目印
    private bool attackForwardFlag = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeState(EnemyState.IDLE);

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        // 毎フレームタイマーが0でないなら減算する
        if (stateTimer > 0)
            stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case EnemyState.IDLE:
                UpdateIdle();
                break;

            case EnemyState.PATROL:
                UpdatePatrol();
                break;

            case EnemyState.MOVE:
                UpdateMove();
                break;

            case EnemyState.ATTACK:
                UpdateAttack();
                break;

            case EnemyState.COOLDOWN:
                UpdateCooldown();
                break;
        }
    }

    void FixedUpdate()
    {
        Debug.Log($"attackForwardFlag = {attackForwardFlag}");
        
        switch (currentState)
        {
            case EnemyState.PATROL:
                Vector2 dirP = (patrolTarget - (Vector2)transform.position).normalized;
                rb.linearVelocity = dirP * moveSpeed;
                break;

            case EnemyState.MOVE:
                Vector2 dirM = (player.position - transform.position).normalized;
                rb.linearVelocity = dirM * moveSpeed;

                // 向きを変える
                if (transform.position.x < player.position.x)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
                break;

            case EnemyState.ATTACK:
                if (attackForwardFlag)
                {
                    Debug.Log("突進！");
                    rb.linearVelocity = attackDirection * attackMoveSpeed;
                }
                else
                {
                    Debug.Log("ノー突進");
                    rb.linearVelocity = Vector2.zero;
                }
                break;

            default:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        Debug.Log($"[EnemyMove] ChangeState：{newState}");
        currentState = newState;
        switch (newState)
        {
            case EnemyState.IDLE:
                stateTimer = idleTime;
                break;

            case EnemyState.PATROL:
                patrolFlag = false;
                stateTimer = 0f;
                break;

            case EnemyState.MOVE:
                break;

            case EnemyState.ATTACK:
                stateTimer = 0f;
                animator.ResetTrigger("attack");
                animator.SetTrigger("attack");

                break;

            case EnemyState.COOLDOWN:
                stateTimer = cooldownTime;

                break;
        }
    }

    private void UpdateIdle()
    {
        // 索敵範囲にプレイヤーが入った時
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            ChangeState(EnemyState.MOVE);
        }
        else if (stateTimer <= 0f)
        {
            ChangeState(EnemyState.PATROL);
        }
    }

    private void UpdatePatrol()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            ChangeState(EnemyState.MOVE);
            return;
        }

        if (!patrolFlag)
        {
            if (stateTimer <= 0f)
            {
                // 動く方向をランダムにきめる
                int dirIdx = Random.Range(0, 4);
                Vector2 dir = dirIdx switch
                {
                    0 => Vector2.up,
                    1 => Vector2.down,
                    2 => Vector2.left,
                    _ => Vector2.right
                };
                // 向きを変える
                if (dir.x != 0)
                    spriteRenderer.flipX = dir.x > 0;

                patrolTarget = (Vector2)transform.position + dir * patrolStepDistance;
                patrolFlag = true;
            }
        }
        else
        {
            // 移動が終わったらIdleに戻る
            if (Vector2.Distance(transform.position, patrolTarget) < 0.05f)
            {
                patrolFlag = false;
                ChangeState(EnemyState.IDLE);
            }
        }
    }

    private void UpdateMove()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > detectionRange)
        {
            ChangeState(EnemyState.IDLE);
        }
        else if (dist <= attackRange)
        {
            ChangeState(EnemyState.ATTACK);
        }
    }

    private void UpdateAttack()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Attack"))
        {
            Debug.Log("スライム、攻撃中");
            return;
        }
        //else if (state.normalizedTime >= 1f)
        //{
        //    ChangeState(EnemyState.COOLDOWN);
        //}
    }

    private void UpdateCooldown()
    {
        Debug.Log("スライム、クールダウン中");
        if (stateTimer <= 0f)
        {
            ChangeState(EnemyState.IDLE);
        }
    }

    // 攻撃の突進の処理（アニメーションとリンク）
    public void AttackMoveForwardStart()
    {
        Debug.Log("突撃開始！");
        attackForwardFlag = true;
        Vector2 dir = (player.position - transform.position).normalized;
        attackDirection = dir;
    }

    public void AttackMoveForwardEnd()
    {
        Debug.Log("突撃終わり");
        attackForwardFlag = false;
        rb.linearVelocity = Vector2.zero;
    }

    // [お試し]アニメーションの終わり
    public void OnAttackAnimationEnd()
    {
        ChangeState(EnemyState.COOLDOWN);
    }
}
