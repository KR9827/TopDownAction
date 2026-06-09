using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }        // どこからでも参照することはできるけど、データをいじることができないインスタンスを生成

    [Header("UI item")]
    [SerializeField] private Image wand;
    [SerializeField] private Image potion;
    [SerializeField] private Image key;
    [SerializeField] private TMP_Text pCount;

    public bool hasWand { get; private set; } = false;
    public bool hasPotion { get; private set; } = false;
    public bool hasKey { get; private set; } = false;
    public int potionCount { get; private set; } = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 最初は非表示
        wand.enabled = false;
        potion.enabled = false;
        key.enabled = false;
        pCount.enabled = false;
        pCount.text = "";
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーン切り替え後にUIを正しく反映
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        wand.enabled = hasWand;
        potion.enabled = hasPotion;
        key.enabled = hasKey;
        pCount.enabled = hasPotion;
    }

    // 杖を獲得した時にほかのスクリプトでよぶ関数
    public void AddWand()
    {
        hasWand = true;
        wand.enabled = true;
        Debug.Log("杖を獲得");
    }

    // 回復薬を獲得した時にほかのスクリプトでよぶ関数
    public void AddPotion()
    {
        hasPotion = true;
        potionCount++;
        potion.enabled = true;
        pCount.enabled = true;
        UpdatePotionUI();
        Debug.Log($"回復薬を獲得 現在{potionCount}個");
    }

    // 回復薬を使うときのよぶ関数
    public void UsePotion(PlayerHealth health)
    {
        if (potionCount > 0)
        {
            potionCount--;
            Debug.Log($"回復薬を使用 現在{potionCount}個");

            // 回復処理
            health.Heal();

            UpdatePotionUI();

            if (potionCount <= 0)
            {
                hasPotion = false;
                potion.enabled = false;
                pCount.enabled = false;
                pCount.text = "";
            }
        }
        else
        {
            Debug.Log("potionがありません");
        }
    }

    private void UpdatePotionUI()
    {
        pCount.text = $"{potionCount}";
    }

    public void AddKey()
    {
        hasKey = true;
        key.enabled = true;
        Debug.Log("鍵を獲得");
    }

    public void UseKey()
    {
        hasKey = false;
        key.enabled = false;
        Debug.Log("鍵を使用");
    }
}
