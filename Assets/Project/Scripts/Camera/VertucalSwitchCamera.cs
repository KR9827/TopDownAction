using UnityEngine;
using Unity.Cinemachine;

public class VerticalSwitchCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera targetCamera1;                    // この通路の下側のカメラ
    [SerializeField] private CinemachineCamera targetCamera2;                    // この通路の上側のカメラ
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがステージを下から上へ移動
            if (other.transform.position.y < transform.position.y)
            {
                targetCamera1.Priority = inactivePriority;
                targetCamera2.Priority = activePriority;
            }
            // プレイヤーがステージを上から下へ移動
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
            // プレイヤーが下に戻る
            if (other.transform.position.y < transform.position.y)
            {
                targetCamera1.Priority = activePriority;
                targetCamera2.Priority = inactivePriority;
            }
            // プレイヤーが上に戻る
            else
            {
                targetCamera1.Priority = inactivePriority;
                targetCamera2.Priority = activePriority;
            }
        }
    }

}
