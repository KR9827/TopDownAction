using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] GameClearUI gameClearUI;
    [SerializeField] GameClearManager gameClearManager;
    BossStatus bossStatus;
    Animator animator;

    void Start()
    {
        bossStatus = GetComponent<BossStatus>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int dmg)
    {
        bossStatus.currentHP -= dmg;
        bossStatus.currentHP = Mathf.Max(bossStatus.currentHP, 0);

        Debug.Log($"currentHP / maxHP：{bossStatus.currentHP},{bossStatus.maxHP}");

        if (bossStatus.currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("死亡");
        animator.SetTrigger("die");
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        Debug.Log("呼ばれた！");
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            yield return null;                  // アニメーションが終わるのを待つ

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        gameObject.SetActive(false);

        if (gameClearUI != null)
        {
            gameClearUI.ShowGameClearUI();            // GameClearUIを表示
        }
        else Debug.Log("ないよーーー");

        gameClearManager.ShowGameClearMenu();
        }

}
