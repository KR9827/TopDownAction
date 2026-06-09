using Unity.Cinemachine;
using UnityEngine;

public class CheckPoint1 : MonoBehaviour
{
    // このチェックポイントがあるステージのカメラ
    [SerializeField] private CinemachineCamera checkPointCam;
    [SerializeField] private Transform resultPos;

    private int id = 2;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointManager.Instance.SetCheckPoint(id, resultPos.position, checkPointCam, "stage_normal");

            // 演出を入れるならここ
            SEManager.Instance.PlaySE("CheckPoint");
            
            Debug.Log("チェックポイント更新");
        }
    }
}
