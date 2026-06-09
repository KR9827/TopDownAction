using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [SerializeField] private GameObject gameOverMenu;

    private bool gameOverFlag = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        gameOverMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        Debug.Log("ShowGameOverMenuが呼ばれた");
        gameOverMenu.SetActive(true);
        gameOverFlag = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameOverFlag && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("押した！");
            OnContinue();
        }
    }

    public void OnContinue()
    {
        Debug.Log("コンティニュー処理開始");
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
        gameOverFlag = false;

        string targetScene = CheckPointManager.Instance.GetLastSceneName();

        // シーンが違う場合ロードする
        if (SceneManager.GetActiveScene().name != targetScene)
        {
            SceneManager.LoadScene(targetScene);
            StartCoroutine(RestoreAfterSceneLoad());
        }
        else
        {
            RestorePlayerAndStage();
        }
    }

    private IEnumerator RestoreAfterSceneLoad()
    {
        yield return null;      // 1フレーム待つ
        RestorePlayerAndStage();
    }

    private void RestorePlayerAndStage()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        int cpID = CheckPointManager.Instance.GetLastCheckPointID();

        // プレイヤーの位置をリセット
        player.transform.position = CheckPointManager.Instance.GetLastCheckPoint();
        Debug.Log("プレイヤーの位置をチェックポイントに移した");

        // プレイヤーのHPをリセット
        player.GetComponent<PlayerHealth>().RestoreFullHP();
        Debug.Log("プレイヤーのHP満タン");

        // チェックポイントによる違い
        switch (cpID)
        {
            case 1:
                Debug.Log("中ボス戦前の状態にする");
                // 中ボスの位置とHPをリセット
                var obstacle = GameObject.FindGameObjectWithTag("GimmickStage4");
                if (obstacle != null)
                {
                    obstacle.GetComponent<Stage4_ObstaclesController>().ResetStage();
                }

                break;

            case 2:
                Debug.Log("ボス戦前の状態にする");
                var lockKey = GameObject.FindGameObjectWithTag("BossStageLock");
                if (lockKey != null)
                {
                    lockKey.SetActive(false);
                }

                var middleBoss = GameObject.FindGameObjectWithTag("MiddleBoss");
                if (middleBoss)
                {
                    middleBoss.SetActive(false);
                }

                var chest = GameObject.FindGameObjectWithTag("Chest");
                if (chest != null)
                {
                    chest.GetComponent<TreasureChest>().ResultChekPoint2();
                }
                break;
        }




        // アニメーション
        var animator = player.GetComponent<Animator>();
        animator.Play("Idle_Forwoad", 0, 0f);

        // プレイヤーがいるステージのカメラに移動
        var cam = CheckPointManager.Instance.GetLastCamera();
        if (cam != null)
        {
            cam.Priority = 30;
            Debug.Log($"復活した時のカメラ = {CheckPointManager.Instance.GetLastCamera()}");
        }
        else
        {
            Debug.Log("nullやで");
        }

        // ゲームに戻るフェード処理
        if (SceneFadeManager.Instance != null)
        {
            StartCoroutine(SceneFadeManager.Instance.FadeIn());
        }
        else
        {
            Debug.Log("nullばい");
        }

        BGMManager.Instance.PlayBGM();
    }
}
