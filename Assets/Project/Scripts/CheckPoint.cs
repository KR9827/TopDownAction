using Unity.Cinemachine;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // このチェックポイントがあるステージのカメラ
    [SerializeField] private CinemachineCamera checkPointCam;

    private int id = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointManager.Instance.SetCheckPoint(id, transform.position, checkPointCam, "stage_normal");

            // 演出を入れるならここ
            SEManager.Instance.PlaySE("CheckPoint");

            Debug.Log("チェックポイント更新");
        }
    }
}
