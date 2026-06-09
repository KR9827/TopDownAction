using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] Collider2D attackCollider1_1;
    [SerializeField] Collider2D attackCollider1_2;
    [SerializeField] Collider2D attackCollider2;
    [SerializeField] BossAttackRangeVisualizer visualizer;


    void Start()
    {
        attackCollider1_1.enabled = false;
        attackCollider1_2.enabled = false;
        attackCollider2.enabled = false;
    }

    // Attack1の当たり判定
    // 1つ目の当たり判定の開始
    public void EnableHitBox1_1()
    {
        attackCollider1_1.enabled = true;
        SEManager.Instance.PlaySE("BossAttack1");
    }
    // 1つ目の当たり判定の終わり
    public void DisableHitBox1_1()
    {
        attackCollider1_1.enabled = false;
    }
    // 2つ目の当たり判定の開始
    public void EnableHitBox1_2()
    {
        attackCollider1_2.enabled = true;
        SEManager.Instance.PlaySE("BossAttack1");
    }
    // 2つ目の当たり判定の終わり
    public void DisableHitBox1_2()
    {
        attackCollider1_2.enabled = false;
    }

    // Attack2の当たり判定
    // Meshの表示開始
    public void EnableMeshRenderer()
    {
        visualizer.ShowMesh();
    }
    // 当たり判定の開始
    public void EnableHitBox2()
    {
        attackCollider2.enabled = true;
        SEManager.Instance.PlaySE("BossAttack2");
    }
    // 当たり判定とMeshの表示の終わり
    public void DisableHitBox2()
    {
        visualizer.HideMesh();
        attackCollider2.enabled = false;
    }

}
