using UnityEngine;

public class GimmickTrigger : MonoBehaviour
{

    [SerializeField] private Sprite beforeGimmick;      // ギミック発動前の画像
    [SerializeField] private Sprite afterGimmick;       // ギミック発動後の画像
    [SerializeField] private GameObject target;         // ギミック発動後に表示するタイルマップ
    [SerializeField] private BoxCollider2D collider2d;  // ギミック発動後にオフにするコライダー


    private SpriteRenderer spriteRenderer;
    private bool activeFlag = false;
    private bool hitFlag = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = beforeGimmick;

        collider2d.enabled = true;

        target.SetActive(false);        
    }


    void Update()
    {
        if (hitFlag && !activeFlag)
            AfterGimmick();
    }


    private void AfterGimmick()
    {
        SEManager.Instance.PlaySE("Gimmick");
        activeFlag = true;
        spriteRenderer.sprite = afterGimmick;

        collider2d.enabled = false;

        target.SetActive(true);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GimmickTrigger"))
        {
            hitFlag = true;
            Destroy(other.gameObject);
        }
    }
}