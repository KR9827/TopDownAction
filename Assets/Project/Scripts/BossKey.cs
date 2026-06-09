using UnityEngine;

public class BossKey : MonoBehaviour
{
    private bool checkFlag = false;
    void Start()
    {

    }

    void Update()
    {
        if (checkFlag && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("取った！");
            ItemManager.Instance.AddKey();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Eで鍵とれるよ");
            checkFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("範囲外");
            checkFlag = false;
        }
    }
}
