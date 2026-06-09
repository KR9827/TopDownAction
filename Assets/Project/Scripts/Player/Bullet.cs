using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Camera mainCamera;
    public float speed = 5f;
    private Vector2 direction = Vector2.up;     // 初期値は上向き

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // プレイヤーの向きに応じて発射する
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 画面外にでたら消す
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPos.y > 1 || viewportPos.y < 0 || viewportPos.x < 0 || viewportPos.x > 1)
        {
            Destroy(gameObject);
        }
    }

    // プレイヤーの向きを受け取る関数
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
}
