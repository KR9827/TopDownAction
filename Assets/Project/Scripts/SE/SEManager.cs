using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [SerializeField] private AudioSource[] seSources;        // 再生用AudioSource
    [SerializeField] private List<SEData> seDataList;        // ScriptableObjectのリスト

    private Dictionary<string, AudioClip> seDict;
    private int nextSourceIndex = 0;

    void Awake()
    {
        // すでに存在していれば重複を破棄
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);      // シーン切り替えでも破棄されない

        // 名前でアクセスできるようにDictionaryに変換
        seDict = new Dictionary<string, AudioClip>();
        foreach (var seData in seDataList)
        {
            if (!seDict.ContainsKey(seData.seName))
                seDict.Add(seData.seName, seData.clip);
        }
    }

    public void PlaySE(string seName)
    {
        if (!seDict.TryGetValue(seName, out AudioClip clip))
        {
            Debug.LogWarning($"SEが見つかりません：{seName}");
            return;
        }

        var src = seSources[nextSourceIndex];
        src.PlayOneShot(clip);

        nextSourceIndex = (nextSourceIndex + 1) % seSources.Length;
    }
}
