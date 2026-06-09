using UnityEngine;

public class BossMove : MonoBehaviour
{
    public enum BossState
    {
        IDLE,
        MOVE_TO_PLAYER,
        ATTACK,
        COOLDOWN
    }

    
    public BossStatus bossStatus;           // ボスのステータスの参照
    public float speedPhase2 = 10f;         // HPが半分以下になった時のボスのスピードにかける数
    public float chaseRange = 5.0f;         // 追跡を始める範囲
    public float attackRange = 2.0f;        // 攻撃判定距離
    public float idleTime = 5.0f;           // 停止時間
    public float cooldownTime = 2.0f;       // 攻撃後の硬直時間

    BossState currentState = BossState.IDLE;
    float stateTimer = 0f;

    [HideInInspector] public bool flip = false;

    Animator animator;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Transform player;                // プライヤーの座標

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        TransitionToState(BossState.IDLE);

        player = Player.Instance.transform;
    }

    void Update()
    {
        // 毎フレームタイマーが0でないなら減算する
        if (stateTimer > 0)
            stateTimer -= Time.deltaTime;

        // 攻撃中とクールダウン以外のとき、プレイヤーの位置によってボスの向きを変える
        if (currentState != BossState.ATTACK && currentState != BossState.COOLDOWN)
        {
            flip = transform.position.x > player.position.x;
            spriteRenderer.flipX = flip;
        }

        switch (currentState)
        {
            case BossState.IDLE:
                UpdateIdle();
                break;
            case BossState.MOVE_TO_PLAYER:
                UpdateMove();
                break;
            case BossState.ATTACK:
                if (bossStatus.currentHP < bossStatus.maxHP / 2)
                    UpdateAttack2();
                else
                    UpdateAttack1();
                break;
            case BossState.COOLDOWN:
                UpdateCooldown();
                break;
        }
    }

    void FixedUpdate()
    {
        // MOVE_TO_PLAYERの時は動く
        if (currentState == BossState.MOVE_TO_PLAYER)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            if (bossStatus.currentHP < bossStatus.maxHP / 2)
            {
                rb.linearVelocity = dir * bossStatus.moveSpeed * speedPhase2;
            }
            else
            {
                rb.linearVelocity = dir * bossStatus.moveSpeed;
            }
        }
        // MOVE_TO_PLAYER以外は移動を止める
        else
        {            
            rb.linearVelocity = Vector2.zero;
        }
    }

    // 状態遷移の共通処理（アニメーション）
    private void TransitionToState(BossState next)
    {
        currentState = next;

        // 状態遷移開始時のタイマーとアニメーション
        switch (next)
        {
            case BossState.IDLE:
                stateTimer = idleTime;
                animator.SetBool("runFlag", false);
                break;
            case BossState.MOVE_TO_PLAYER:
                stateTimer = 0f;
                animator.SetBool("runFlag", true);
                break;
            case BossState.ATTACK:
                stateTimer = 0f;
                if (bossStatus.currentHP < bossStatus.maxHP / 2)
                    animator.SetTrigger("attack2");
                else
                    animator.SetTrigger("attack1");
                break;
            case BossState.COOLDOWN:
                stateTimer = cooldownTime;
                animator.SetBool("runFlag", false);
                break;
        }
    }

    private void UpdateIdle()
    {
        // 停止時間が終わったら次の状態に遷移する
        if (stateTimer <= 0f)
        {
            TransitionToState(BossState.MOVE_TO_PLAYER);
        }
    }

    // プレイヤーに向かって移動
    private void UpdateMove()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange)
            TransitionToState(BossState.ATTACK);
    }

    private void UpdateAttack1()
    {
        //Debug.Log("attack1");

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Attack") && state.normalizedTime < 1f)
            return;
        else
            TransitionToState(BossState.COOLDOWN);
    }

    private void UpdateAttack2()
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
                TransitionToState(BossState.COOLDOWN);
            }
    }

    private void UpdateCooldown()
    {
        // 攻撃後の硬直終わり
        if (stateTimer <= 0f)
        {
            animator.speed = 1f;
            TransitionToState(BossState.IDLE);
        }
    }
}
