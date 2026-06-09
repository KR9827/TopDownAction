using UnityEngine;
using UnityEngine.UI;

public class BulletCooldownUI : MonoBehaviour
{
    [SerializeField] private Image coverImage;

    private RectTransform coverRect;

    private float cooldownTime;
    private float cooldownTimer;
    private float fullHeight;

    void Start()
    {
        coverRect = coverImage.GetComponent<RectTransform>();
        fullHeight = coverRect.sizeDelta.y;                     // 最初の高さを保持
        coverImage.gameObject.SetActive(false);                 // 最初は非表示
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            float ratio = Mathf.Clamp01(cooldownTimer / cooldownTime);
            coverRect.sizeDelta = new Vector2(coverRect.sizeDelta.x, fullHeight * ratio);

            if (cooldownTimer <= 0f)
            {
                coverImage.gameObject.SetActive(false);     // クールダウン終了で非表示
            }
        }
    }

    public void StartCooldown(float time)
    {
        cooldownTime = time;
        cooldownTimer = time;
        coverImage.gameObject.SetActive(true);
    }
}
