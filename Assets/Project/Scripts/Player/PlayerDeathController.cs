using UnityEngine;
using System.Collections;

public class PlayerDeathController : MonoBehaviour
{
    Animator animator;
    private PlayerSceneChange sceneChange;

    void Start()
    {
        animator = GetComponent<Animator>();
        sceneChange = GetComponent<PlayerSceneChange>();
    }

    public void Die()
    {
        Debug.Log("死亡");
        animator.SetTrigger("die");

        if (sceneChange != null)
        {
            sceneChange.SetGameOverMode(true);
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
            yield return null;                  // アニメーションが終わるのを待つ

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        if (GameOverManager.Instance != null)
        {
            yield return SceneFadeManager.Instance.FadeOut();       // フェードアウト
            BGMManager.Instance.StopBGM();
            SEManager.Instance.PlaySE("GameOver");
            GameOverManager.Instance.ShowGameOverMenu();             // GameOverUIを表示
        }
        else
        {
            Debug.Log("nullだね");
        }
    }
}
