using UnityEngine;

public class EnemyAttackManager2 : MonoBehaviour
{
    public Collider2D attackRange;

    void Start()
    {
        attackRange.enabled = false;
    }

    // 当たり判定
    public void EnableAttackRange()
    {
        attackRange.enabled = true;
    }
    public void DisableAttackRange()
    {
        attackRange.enabled = false;
    }

}
