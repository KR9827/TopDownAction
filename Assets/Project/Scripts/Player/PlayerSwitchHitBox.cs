using UnityEngine;

public class PlayerSwitchHitBox : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    BoxCollider2D box;
    private Vector2 originalOffset;
    void Start()
    {
        box = GetComponent<BoxCollider2D>();

        originalOffset = box.offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = originalOffset;
        // プレイヤーの向きに合わせてコライダーを反転
        if (spriteRenderer.flipX)
            offset.x = -originalOffset.x;

        box.offset = offset;
    }
}
