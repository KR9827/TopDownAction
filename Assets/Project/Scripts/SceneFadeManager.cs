using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class SceneFadeManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject gameOverUI;

    public static SceneFadeManager Instance { get; private set; }
    public bool IsFlag { get; private set; }

    public event Action OnFadeOutStart;         // event：ほかのスクリプトに対して、特定のタイミングを知らせる仕組み
    public event Action OnFadeOutEnd;
    public event Action OnFadeInStart;
    public event Action OnFadeInEnd;


    private void Awake()
    {
        // すでに存在していれば重複を破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);      // シーン切り替えでも破棄されない

        // 初期状態は透明から
        if (fadeCanvasGroup != null) fadeCanvasGroup.alpha = 0f;
        if (gameOverUI != null) gameOverUI.SetActive(false);
    }

    // 違うスクリプトで使う関数
    public void ChangeSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadGameScene(sceneName));
    }

    public IEnumerator ChangeSceneWithFadeRoutine(string sceneName)
    {
        yield return LoadGameScene(sceneName);
    }

    private IEnumerator LoadGameScene(string sceneName)
    {
        yield return FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneName);
        // 次のシーンの初期化が1フレーム進むのを待つとチラつきを防止できる
        yield return null;
        yield return FadeIn();
    }

    public IEnumerator FadeIn()
    {
        OnFadeInStart?.Invoke();            // ?.：nullじゃない時実行する、という意味
                                            // Invoke()：「デリゲート」「イベント」を実行するためのメソッド
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;         // ゲーム時間が止まっていても使える
            fadeCanvasGroup.alpha = 1f - (time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;

        OnFadeInEnd?.Invoke();
    }

    public IEnumerator FadeOut()
    {
        //if (invokeEvents)
            OnFadeOutStart?.Invoke();

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = time / fadeDuration;
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;

        //if (invokeEvents)
            OnFadeOutEnd?.Invoke();
    }

    // GameOverUIの表示/非表示をほかのスクリプトで行うための関数
    //public void ShowGameOverUI()
    //{
    //    if (gameOverUI == null)
    //    {
    //        Debug.LogWarning("SceneFadeManager：gameOverUIがセットされていません。");
    //        return;
    //    }
//
    //    gameOverUI.SetActive(true);
    //}
//
    //public void HideGameOverUI()
    //{
    //    if (gameOverUI == null)
    //        return;
//
    //    gameOverUI.SetActive(false);
    //}

}
