using UnityEngine;

public class BossAttackHit2 : MonoBehaviour
{
    public int baseDamage = 3;
    public BossStatus bossStatus;
    int damage = 0;

    void Start()
    {
        damage = baseDamage * bossStatus.attackPower;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤー以外は無視
            if (!other.CompareTag("Player"))
                return;

            Debug.Log("攻撃2が当たった！");
            // ここでダメージ計算する
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
