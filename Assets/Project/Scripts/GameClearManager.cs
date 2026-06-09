using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] GameClearUI gameClearUI;
    private bool gameClearFlag = false;

    void Start()
    {

    }

    public void ShowGameClearMenu()
    {
        Debug.Log("ShowGameOverMenuが呼ばれた");
        gameClearFlag = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        //if (gameClearFlag && Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("押した！");
        //    OnTitle();
        //}
    }

    private void OnTitle()
    {
        Debug.Log("コンティニュー処理開始");
        StartCoroutine(ReturnToTitle());
        
    }

    private IEnumerator ReturnToTitle()
    {
        Time.timeScale = 1f;
        gameClearUI.HideGameClearUI();
        gameClearFlag = false;

        // ゲームに戻るフェード処理
        if (SceneFadeManager.Instance != null)
        {
            yield return SceneFadeManager.Instance.ChangeSceneWithFadeRoutine("Title");
        }
    }
}
