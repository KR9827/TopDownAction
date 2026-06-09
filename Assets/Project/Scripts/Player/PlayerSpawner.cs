using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void OnEnable()
    {
        // シーン読み込み完了イベントに登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // SpawnPointを探す
        GameObject spawn = GameObject.FindWithTag("PlayerSpawnPoint");
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
        }
    }


}
