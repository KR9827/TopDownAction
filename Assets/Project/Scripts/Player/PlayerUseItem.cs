using UnityEngine;
using UnityEngine.UI;

public class PlayerUseItem : MonoBehaviour
{
    [Header("=== Bullet Prafab")]
    public GameObject bulletPrefab;
    [Header("=== Bullet Cooldown UI")]
    [SerializeField] private BulletCooldownUI cooldownUI;

    PlayerStatus playerStatus;
    PlayerHealth health;    

    // 弾のインターバル用
    private float nextFireTime = 0f;
    public float fireCooldown = 1f;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Pで魔法弾発射
        if (Input.GetKeyDown(KeyCode.P) && ItemManager.Instance.hasWand)
        {
            // 撃てるかどうかチェック
            if (Time.time >= nextFireTime)
            {
                Shoot();
                // 次に撃てる時間を設定
                nextFireTime = Time.time + fireCooldown;

                // UI側にクールダウン開始を伝える
                if (cooldownUI != null)
                {
                    cooldownUI.StartCooldown(fireCooldown);
                }
            }
            else
            {
                Debug.Log("クールダウン中");
            }
        }

        void Shoot()
        {
            Debug.Log("発射！！");
            SEManager.Instance.PlaySE("Shot");

            // prefabの弾を生成する
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // プレイヤーの向きをBulletに渡す
            bullet.GetComponent<Bullet>().SetDirection(Player.Instance.lastDirection);
        }

        // Qで回復薬を使用
        if (Input.GetKeyDown(KeyCode.Q) && ItemManager.Instance.hasPotion && playerStatus.currentHP != playerStatus.maxHP)
        {
            ItemManager.Instance.UsePotion(health);
            SEManager.Instance.PlaySE("Potion");
        }
    }
}
