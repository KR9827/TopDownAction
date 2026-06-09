using Unity.VisualScripting;
using UnityEngine;

public class Stage4_ObstaclesController : MonoBehaviour
{
    [SerializeField] MiddleBossStatus middleBossStatus;
    [SerializeField] GameObject target;
    [SerializeField] MiddleBossMove move;
    [SerializeField] Transform middleBoss;
    [SerializeField] Transform middleBossSpawnPoint;

    private bool triggerFlag = false;

    void Start()
    {
        ResetStage();
    }

    void Update()
    {
        if (middleBossStatus.currentHP <= 0 && target.activeSelf)
            target.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target.SetActive(true);
            move.enabled = true;
            triggerFlag = true;
        }
    }

    // リトライ時にリセット処理する関数
    public void ResetStage()
    {
        target.SetActive(false);
        move.enabled = false;
        triggerFlag = false;

        // 中ボスのHPをリセット
        if (middleBossStatus != null)
        {
            middleBossStatus.currentHP = middleBossStatus.maxHP;
        }
        // 中ボスの位置をリセット
        if (middleBoss != null && middleBossSpawnPoint != null)
        {
            middleBoss.position = middleBossSpawnPoint.position;
        }
    }
}
