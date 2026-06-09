using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    EnemyStatus1 eStatus1;
    EnemyStatus2 eStatus2;
    Animator animator;

    void Start()
    {
        eStatus1 = GetComponent<EnemyStatus1>();
        eStatus2 = GetComponent<EnemyStatus2>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        if (eStatus1 != null)
        {
            eStatus1.currentHP -= dmg;
            eStatus1.currentHP = Mathf.Max(eStatus1.currentHP, 0);

            Debug.Log($"currentHP/maxHP：{eStatus1.currentHP}/{eStatus1.maxHP}");

            if (eStatus1.currentHP <= 0)
            {
                Die();
            }
        }

        if (eStatus2 != null)
        {
            eStatus2.currentHP -= dmg;
            eStatus2.currentHP = Mathf.Max(eStatus2.currentHP, 0);

            if (eStatus2.currentHP <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log("スライム死亡");
        animator.SetTrigger("die");
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
            yield return null;                  // アニメーションが終わるのを待つ

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        Destroy(gameObject);
    }
}
