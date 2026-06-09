using UnityEngine;

public class MiddleBossAttackHit : MonoBehaviour
{
    public int baseDamage = 2;
    public MiddleBossStatus middleBossStatus;
    int damage = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーに攻撃が当たった");

            var health = other.GetComponent<PlayerHealth>();
            var status = other.GetComponent<PlayerStatus>();
            if (health != null && status != null)
            {
                int defence = status.defencePower;
                damage = (middleBossStatus.attackPower - defence) * baseDamage;
                health.TakeDamage(damage);
            }

        }
    }
}
