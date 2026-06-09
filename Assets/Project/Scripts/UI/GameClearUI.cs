using UnityEngine;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private GameObject gameClearUI;

    private void Awake()
    {
        if (gameClearUI != null) gameClearUI.SetActive(false);
    }

    // GameClearUIの表示/非表示をほかのスクリプトで行うための関数
    public void ShowGameClearUI()
    {
        Debug.Log("呼ばれたよ");
        if (gameClearUI == null)
        {
            Debug.LogWarning("SceneFadeManager：gameClearUIがセットされていません。");
            return;
        }

        gameClearUI.SetActive(true);
        BGMManager.Instance.StopBGM();
        SEManager.Instance.PlaySE("GameClear");
    }

    public void HideGameClearUI()
    {
        if (gameClearUI == null)
        {
            return;
        }

        gameClearUI.SetActive(false);
    }
}
