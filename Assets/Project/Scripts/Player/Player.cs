using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    // インスペクターに表示
    public float moveSpeed = 5.0f;
    public float dodgeSpeed = 7.0f;
        

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    PlayerHealth health;
    PlayerSceneChange sceneChange;

    float lastHorizontalTime = 0.0f;
    float lastVerticalTime = 0.0f;    

    // 入力を一時的に保持
    Vector2 input;
    public Vector2 lastDirection { get; private set; } = Vector2.down;       // 最後に向いていた方向（初期は下）
    Vector2 dodgeDirection;    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
        sceneChange = GetComponent<PlayerSceneChange>();
    }

    void Update()   // 描画フレーム依存の更新処理（60fps?）
    {
        // フェード処理中は停止する
        switch (sceneChange.fadeBehavior)
        {
            case PlayerSceneChange.FadeBehavior.FixedMove:
                input = Vector2.up;
                animator.SetBool("moveFlag", true);

                return;
            case PlayerSceneChange.FadeBehavior.Stop:
                input = Vector2.zero;
                animator.SetBool("moveFlag", false);

                return;
        }

        if (health.IsInvincible)
        {
            Debug.Log("無敵中");
            return;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);
        // 攻撃中ならなにもしない
        if (state.IsTag("Attack"))
        {
            return;
        }

        // エンターキーで攻撃
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (lastDirection.y < 0) animator.SetTrigger("attackForward");
            else if (lastDirection.y > 0) animator.SetTrigger("attackBack");
            else animator.SetTrigger("attackSide");

            SEManager.Instance.PlaySE("PlayerAttack");
            return;
        }

        // 入力があったら、その時刻を記録する
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            lastHorizontalTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            lastVerticalTime = Time.time;
        }


        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 右向き
        if (h > 0)
        {
            spriteRenderer.flipX = false;
            lastDirection = Vector2.right;
        }
        // 左向き
        if (h < 0)
        {
            spriteRenderer.flipX = true;
            lastDirection = Vector2.left;
        }
        // 上向き
        if (v > 0) lastDirection = Vector2.up;
        // 下向き
        if (v < 0) lastDirection = Vector2.down;

        // 両方入力された場合、最後に押された方を優先する
        if (h != 0 && v != 0)
        {
            if (lastHorizontalTime > lastVerticalTime) v = 0;   // 横の方が新しい->縦を0にする
            else h = 0;                                         // 縦の方が新しい->横を0にする
        }

        input = new Vector2(h, v).normalized;           // FixedUpdateで使うために保存

        // アニメーション管理
        bool isMoving = input.magnitude > 0;            // 移動しているかを確認
        animator.SetBool("moveFlag", isMoving);         // 移動していたらAnimatorのフラグを立てる

        // 方向ごとのアニメーション用フラグ
        animator.SetBool("runForwardFlag", v < 0);
        animator.SetBool("runBackFlag", v > 0);
        animator.SetBool("runSideFlag", h != 0);

        // Spaceでドッジ
        if ((v != 0 || h != 0) && Input.GetKeyDown(KeyCode.Space))
        {
            if (lastDirection.y < 0) animator.SetTrigger("dodgeForward");
            else if (lastDirection.y > 0) animator.SetTrigger("dodgeBack");
            else animator.SetTrigger("dodgeSide");

            SEManager.Instance.PlaySE("Dodge");
            // 最後の方向を固定して転がる
            dodgeDirection = lastDirection.normalized;
        }        
    }

    void FixedUpdate()  // 物理フレーム依存の更新処理（50fps?）
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);

        // 攻撃中は動かない
        if (state.IsTag("Attack"))
        {
            rb.linearVelocity = Vector2.zero;

            return;
        }
        
        // ドッジのアニメーション中のとき
        if (state.IsTag("Dodge"))
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;

            return;         // ドッジ中は通常移動しない
        }

        // 通常移動
        rb.linearVelocity = input * moveSpeed;
    }
}
