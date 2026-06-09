using UnityEngine;

public class SceneChange_BOSS : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // フェードアウト、シーン遷移のコルーチン処理
            SceneFadeManager.Instance.ChangeSceneWithFade("stage_BOSS");
        }
    }
}