using System;
using UnityEngine;

public class PlayerSceneChange : MonoBehaviour
{
    public enum FadeBehavior
    {
        None,           // 通常時
        Stop,           // フェードアウト時：停止
        FixedMove,      // フェードアウト時：特定の動きをさせる
    }
    public FadeBehavior fadeBehavior { get; private set; }

    private bool gameOverModeFlag = false; 

    void Start()
    {
        fadeBehavior = FadeBehavior.None;
    }

    void OnEnable()
    {
        var mgr = SceneFadeManager.Instance;
        if (mgr == null) return;

        mgr.OnFadeOutStart += HandleFadeOutStart;
        mgr.OnFadeInStart += HandleFadeInStart;
        mgr.OnFadeInEnd += HandleFadeInEnd;
    }

    void OnDisable()
    {
        var mgr = SceneFadeManager.Instance;
        if (mgr == null) return;

        mgr.OnFadeOutStart -= HandleFadeOutStart;
        mgr.OnFadeInStart -= HandleFadeInStart;
        mgr.OnFadeInEnd -= HandleFadeInEnd;
    }

    // gameOver時に呼ばれる
    public void SetGameOverMode(bool value)
    {
        gameOverModeFlag = value;
    }

    private void HandleFadeOutStart()
    {
        if (gameOverModeFlag)
            fadeBehavior = FadeBehavior.Stop;       // 死亡時は停止
        else
            fadeBehavior = FadeBehavior.FixedMove;  // シーン切り替えの時は自動で動く
    }

    private void HandleFadeInStart()
    {
        fadeBehavior = FadeBehavior.Stop;
    }

    private void HandleFadeInEnd()
    {
        fadeBehavior = FadeBehavior.None;

        // gameOverModeをリセット
        gameOverModeFlag = false;
    }
}
