using System.Collections.Generic;
using UnityEngine;

public class BossAttackHit1 : MonoBehaviour
{
    public int damage = 10;        // ダメージ量


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤー以外は無視
            if (!other.CompareTag("Player"))
                return;

            Debug.Log("攻撃1が当たった！");
            // ここでダメージ計算する
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
