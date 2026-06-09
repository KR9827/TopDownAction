using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CinemachineFollow : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        // FindObjectByTipeで配列を取得
        var cams = Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);

        // シーン内のすべてのCamに設定し直す
        foreach (var cam in cams)
        {
            cam.Follow = player;
        }
    }
}
