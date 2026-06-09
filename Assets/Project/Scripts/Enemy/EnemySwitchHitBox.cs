using UnityEngine;

public class EnemySwitchHitBox : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    Collider2D col;
    private Vector2 originalOffset;
    void Start()
    {
        col = GetComponent<Collider2D>();

        originalOffset = col.offset;
    }

        void Update()
    {
        Vector2 offset = originalOffset;
        // プレイヤーの向きに合わせてコライダーを反転
        if (spriteRenderer.flipX)
            offset.x = -originalOffset.x;

        col.offset = offset;
    }
}
