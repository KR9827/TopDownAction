using UnityEngine;

public class MiddleBossAttackManager : MonoBehaviour
{
    public Collider2D attackRange;
    public MiddleBossAttackRangeVisualizer visualizer;
    void Start()
    {
        attackRange.enabled = false;
    }

    // 当たり判定
    public void EnableShowMesh()
    {
        Debug.Log("Mesh表示");
        visualizer.ShowMesh();
    }
    public void EnableAttackRange()
    {
        Debug.Log("攻撃開始");
        attackRange.enabled = true;
        SEManager.Instance.PlaySE("MiddleBossAttack");
    }
    public void DisableAttackRange()
    {
        Debug.Log("攻撃終わり");
        visualizer.HideMesh();
        attackRange.enabled = false;
    }
}
