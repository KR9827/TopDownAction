using System.Collections;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField] private Sprite closedChest;            // 閉じた宝箱の画像
    [SerializeField] private Sprite openChest;              // 開いた宝箱の画像
    [SerializeField] private KeyCode openKey = KeyCode.E;   // 開けるキー
    [SerializeField] private GameObject magicWand;          // 宝箱の中身

    private SpriteRenderer spriteRenderer;
    private bool openFlag = false;
    private bool playerInRengeFlag = false;


    private float speed = 0.5f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedChest;

        magicWand.SetActive(false);
    }

    void Update()
    {
        if (playerInRengeFlag && !openFlag && Input.GetKeyDown(openKey))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        openFlag = true;
        spriteRenderer.sprite = openChest;
        SEManager.Instance.PlaySE("OpenChest");

        // 宝箱からアイテムが飛び出す
        magicWand.SetActive(true);
        StartCoroutine(RiseItem(magicWand));

        // -------------------------------
        // ここでアイテム獲得などの処理を行う
        // -------------------------------
        if (magicWand.name.Contains("MagicWand"))
        {
            ItemManager.Instance.AddWand();
        }


        Debug.Log("宝箱を開けた！");
    }


    private IEnumerator RiseItem(GameObject item)
    {
        float riseTime = 1f;
        float nowTime = 0f;
        Vector3 startPos = item.transform.position;

        while (nowTime < riseTime)
        {
            nowTime += Time.deltaTime;
            item.transform.position = startPos + Vector3.up * (nowTime * speed);
            yield return null;
        }

        Destroy(item);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRengeFlag = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRengeFlag = false;
    }

    public void ResultChekPoint2()
    {
        spriteRenderer.sprite = openChest;
        magicWand.SetActive(false);
    }
}


