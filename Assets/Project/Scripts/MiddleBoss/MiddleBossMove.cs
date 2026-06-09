using UnityEngine;

public class MiddleBossMove : MonoBehaviour
{
    public enum MiddleBossState
    {
        IDLE,
        MOVE_TO_PLAYER,
        ATTACK,
        COOLDOWN
    }

    public Transform player;                    // プライヤーの座標
    public MiddleBossStatus middleBossStatus;   // ボスのステータスの参照
    public float attackRange = 2.0f;            // プレイヤーの近づいて止まる距離
    public float idleTime = 5.0f;               // 停止時間
    public float cooldownTime = 2.0f;           // 攻撃後の硬直時間

    MiddleBossState currentState = MiddleBossState.IDLE;
    float stateTimer = 0f;

    [HideInInspector] public bool flip = false;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionToState(MiddleBossState.IDLE);
    }

    void Update()
    {
        // 毎フレームタイマーが0でないなら減算する
        if (stateTimer > 0)
            stateTimer -= Time.deltaTime;

        // 攻撃中とクールダウン以外のとき、プレイヤーの位置によってボスの向きを変える
        if (currentState != MiddleBossState.ATTACK && currentState != MiddleBossState.COOLDOWN)
        {
            flip = transform.position.x < player.position.x;
            spriteRenderer.flipX = flip;
        }

        switch (currentState)
        {
            case MiddleBossState.IDLE:
                UpdateIdle();
                break;
            case MiddleBossState.MOVE_TO_PLAYER:
                UpdateMove();
                break;
            case MiddleBossState.ATTACK:
                //if (middleBossStatus.currentHP > middleBossStatus.maxHP / 2)
                //UpdateAttack2();
                //else
                    UpdateAttack();
                break;
            case MiddleBossState.COOLDOWN:
                UpdateCooldown();
                break;
        }
    }

    // MiddleBossの動き
    void FixedUpdate()
    {
        // MOVE_TO_PLAYERの時は動く
        if (currentState == MiddleBossState.MOVE_TO_PLAYER)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * middleBossStatus.moveSpeed;
        }
        // MOVE_TO_PLAYER以外は移動を止める
        else
        {            
            rb.linearVelocity = Vector2.zero;
        }
    }

    // 状態遷移の共通処理
    private void TransitionToState(MiddleBossState next)
    {
        currentState = next;

        // 状態遷移開始時のタイマーとアニメーション
        switch (next)
        {
            case MiddleBossState.IDLE:
                stateTimer = idleTime;
                //animator.SetBool("runFlag", false);
                break;
            case MiddleBossState.MOVE_TO_PLAYER:
                stateTimer = 0f;
                //animator.SetBool("runFlag", true);
                break;
            case MiddleBossState.ATTACK:
                stateTimer = 0f;
                //if (middleBossStatus.currentHP > middleBossStatus.maxHP / 2){}
                    //animator.SetTrigger("attack2");
                //else
                    animator.SetTrigger("attack");
                break;
            case MiddleBossState.COOLDOWN:
                stateTimer = cooldownTime;
                //animator.SetBool("runFlag", false);
                break;
        }
    }

    private void UpdateIdle()
    {
        // 停止時間が終わったら次の状態に遷移する
        if (stateTimer <= 0f)
        {
            TransitionToState(MiddleBossState.MOVE_TO_PLAYER);
        }
    }

    private void UpdateMove()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange)
            TransitionToState(MiddleBossState.ATTACK);
    }

    private void UpdateAttack()

    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsTag("Attack") && state.normalizedTime < 1f)
            {
                return;
            }
            else
            {
                // 攻撃アニメの最後のフレームに固定
                animator.Play(state.fullPathHash, 0, 1f);       // Animatorの特定のステートを再生する。fullPathHash：今再生中のアニメーションを指定、第三引数　1.0ｆ：アニメーションの最後のフレーム
                animator.speed = 0f;
                TransitionToState(MiddleBossState.COOLDOWN);
            }
    }

    private void UpdateCooldown()
    {
        // 攻撃後の硬直終わり
        if (stateTimer <= 0f)
        {
            animator.speed = 1f;
            TransitionToState(MiddleBossState.IDLE);
        }
    }
}
