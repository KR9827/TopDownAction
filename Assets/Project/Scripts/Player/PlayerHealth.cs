using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class PlayerHealth : MonoBehaviour
{
    PlayerStatus playerStatus;
    PlayerDeathController deathController;
    SpriteRenderer spriteRenderer;

    bool invincibleFlag = false;     // 無敵フラグ

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        deathController = GetComponent<PlayerDeathController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int dmg)
    {
        if (invincibleFlag) return;  // 無敵中

        playerStatus.currentHP -= dmg;
        playerStatus.currentHP = Mathf.Max(playerStatus.currentHP, 0);      // HPが負にならないようにする

        Debug.Log($"player currentHP / maxHP : {playerStatus.currentHP} / {playerStatus.maxHP}");

        // ここにエフェクトとか書いてもいい

        if (playerStatus.currentHP <= 0)
        {
            deathController.Die();
        }
        else
        {
            // 無敵モードへ移行
            StartCoroutine(DamageInvincible());
        }
    }

    // 回復薬を使ったとき、最大HPの1/3回復
    public void Heal()
    {
        int healAmount = Mathf.RoundToInt(playerStatus.maxHP * 0.3f);
        playerStatus.currentHP += healAmount;
        // 最大HPをこえないようにする
        playerStatus.currentHP = Mathf.Min(playerStatus.currentHP, playerStatus.maxHP);
        Debug.Log($"HPを回復　currentHP / maxHP：{playerStatus.currentHP},{playerStatus.maxHP}");
    }

    // ダメージを受けたときの処理
    private IEnumerator DamageInvincible()
    {
        invincibleFlag = true;

        float invincibleTime = 0.5f;
        float blinkInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;      // 最後は表示に戻す
        invincibleFlag = false;
    }

    public bool IsInvincible => invincibleFlag;     // 外部から読み取ることができる

    public void RestoreFullHP()
    {
        playerStatus.currentHP = playerStatus.maxHP;
    }
}
