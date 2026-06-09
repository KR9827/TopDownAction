using UnityEngine;
using Unity.Cinemachine;

public class SideSwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera1;                    // この通路の左側のカメラ
    [SerializeField] private CinemachineCamera targetCamera2;                    // この通路の右側のカメラ
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがステージを左から右へ移動
            if (other.transform.position.x < transform.position.x)
            {
                targetCamera1.Priority = inactivePriority;
                targetCamera2.Priority = activePriority;
            }
            // プレイヤーがステージを右から左へ移動
            else
            {
                targetCamera1.Priority = activePriority;
                targetCamera2.Priority = inactivePriority;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーが左に戻る
            if (other.transform.position.x < transform.position.x)
            {
                targetCamera1.Priority = activePriority;
                targetCamera2.Priority = inactivePriority;
            }
            // プレイヤーが右に戻る
            else
            {
                targetCamera1.Priority = inactivePriority;
                targetCamera2.Priority = activePriority;
            }
        }
    }

}
