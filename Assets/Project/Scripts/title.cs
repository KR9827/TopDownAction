using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class title : MonoBehaviour
{
    [SerializeField] GameObject titleUI;

    void Start()
    {
        titleUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // SceneFadeManagerの関数で遷移する
            SceneFadeManager.Instance.ChangeSceneWithFade("stage_Normal");
            HideTitleUI();
            SEManager.Instance.PlaySE("GameStart");
        }
    }

    public void HideTitleUI()
    {
        if (titleUI == null)
            return;

        titleUI.SetActive(false);
    }
}
