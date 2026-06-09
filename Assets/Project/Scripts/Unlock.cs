using UnityEngine;

public class Unlock : MonoBehaviour
{
    public Sprite unlockSprite;
    SpriteRenderer sprite;
    private bool checkFlag = false;
    private bool unlockFlag = false;
    private float timer = 0f;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (checkFlag && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("解除した");
            SEManager.Instance.PlaySE("OpenKey");
            unlockFlag = true;
            timer = 0f;            
            ItemManager.Instance.UseKey();
            sprite.sprite = unlockSprite;            
        }

        if (unlockFlag)
        {
            timer += Time.deltaTime;

            // 1秒経ったら非アクティブ化
            if (timer >= 1f)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ItemManager.Instance.hasKey)
        {
            Debug.Log("解除できるよ");
            checkFlag = true;
        }
    }

        private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ItemManager.Instance.hasKey)
        {
            Debug.Log("範囲外");
            checkFlag = false;
        }
    }
}
