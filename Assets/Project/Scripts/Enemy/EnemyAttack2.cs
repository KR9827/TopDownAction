using UnityEngine;

public class EnemyAttack2 : MonoBehaviour
{
    public int baseDamage = 2;
    public EnemyStatus2 status2;
    int damage = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに攻撃が当たった");
            SEManager.Instance.PlaySE("EnemyAttack");

            var health = other.GetComponent<PlayerHealth>();
            var status = other.GetComponent<PlayerStatus>();
            if (health != null && status != null)
            {
                int defence = status.defencePower;
                damage = (status2.attackPower - defence) * baseDamage;
                health.TakeDamage(damage);
            }

        }
    }
}
