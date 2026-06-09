using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MiddleBossHealth : MonoBehaviour
{
    [SerializeField] GameObject bossKey;
    MiddleBossStatus middleBossStatus;
    Animator animator;

    private float speed = 0.5f;

    void Start()
    {
        middleBossStatus = GetComponent<MiddleBossStatus>();
        animator = GetComponent<Animator>();
        bossKey.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        middleBossStatus.currentHP -= dmg;
        middleBossStatus.currentHP = Mathf.Max(middleBossStatus.currentHP, 0);

        Debug.Log($"currnetHP / maxHP：{middleBossStatus.currentHP} / {middleBossStatus.maxHP}");
        if (middleBossStatus.currentHP <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("死亡");
        animator.SetTrigger("die");

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // アニメーションがDieになるまで待つ
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            yield return null;

        // アニメーションが終わるまで待つ
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // アニメーションの最後のフレームで止める
        //animator.Play("Die", 0, 1f);
        //animator.speed = 0f;

        // アイテムを出す
        yield return StartCoroutine(RiseKey());

        // middleBossを非アクティブ化
        DisableSetActive();
    }

    private IEnumerator RiseKey()
    {
        float riseTime = 1f;
        float nowTime = 0f;

        Vector3 startPos = transform.position;
        bossKey.transform.position = startPos;        // middleBossが倒れた場所
        bossKey.SetActive(true);

        while (nowTime < riseTime)
        {
            nowTime += Time.deltaTime;
            bossKey.transform.position = startPos + Vector3.up * (nowTime * speed);
            yield return null;
        }
    }

    public void DisableSetActive()
    {
        gameObject.SetActive(false);
    }
}
