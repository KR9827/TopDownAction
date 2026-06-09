using UnityEngine;

public class EnemyAttack1 : MonoBehaviour
{
    public int baseDamage = 2;
    public EnemyStatus1 status1;
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
                damage = (status1.attackPower - defence) * baseDamage;
                health.TakeDamage(damage);
            }

        }
    }
}
