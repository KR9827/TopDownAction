using UnityEngine;

[CreateAssetMenu(fileName = "SEData", menuName = "Sound/SEData")]
public class SEData : ScriptableObject
{
    public string seName;       // SEの名前
    public AudioClip clip;      // 音声ファイル
}
