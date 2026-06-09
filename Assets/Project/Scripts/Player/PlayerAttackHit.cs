using UnityEngine;

public class PlayerAttackHit : MonoBehaviour
{
    public int baseDamage = 3;
    public PlayerStatus playerStatus;
    int damage = 0;
    
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MiddleBoss"))
        {
            // ダメージ計算用のスプライトを取得
            var healthMiddleBoss = other.GetComponent<MiddleBossHealth>();

            // MiddleBossに攻撃が当たった時
            if (healthMiddleBoss != null)
            {
                var status = other.GetComponent<MiddleBossStatus>();
                if (status != null)
                {
                    int defence = status.defencePower;
                    damage = (playerStatus.attackPower - defence) * baseDamage;
                    healthMiddleBoss.TakeDamage(damage);
                }
            }
        }

        if (other.CompareTag("Enemy"))
        {
            // ダメージ計算用のスプライトを取得
            var healthBoss = other.GetComponent<BossHealth>();
            var healthEnemy = other.GetComponent<EnemyHealth>();

            // Bossに攻撃が当たった時
            if (healthBoss != null)
            {
                var status = other.GetComponent<BossStatus>();
                if (status != null)
                {
                    int defence = status.defencePower;
                    damage = (playerStatus.attackPower - defence) * baseDamage;
                    healthBoss.TakeDamage(damage);
                }
            }

            // Enemyに攻撃が当たった時
            if (healthEnemy != null)
            {
                Debug.Log("ステータスを取得");
                var status1 = other.GetComponent<EnemyStatus1>();
                var status2 = other.GetComponent<EnemyStatus2>();

                if (status1 != null)
                {
                    Debug.Log("アタック！");
                    int defence = status1.defencePower;
                    damage = (playerStatus.attackPower - defence) * baseDamage;
                    healthEnemy.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("nullになってますよ");
                }

                if (status2 != null)
                {
                    Debug.Log("アタック2！");
                    int defence = status2.defencePower;
                    damage = (playerStatus.attackPower - defence) * baseDamage;
                    healthEnemy.TakeDamage(damage);
                }
            }
            else
            {
                Debug.Log("nullですよ");
            }
        }
    }


}
