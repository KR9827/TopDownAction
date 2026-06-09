using UnityEngine;

public class BossStatus : MonoBehaviour
{
    // インスペクターに表示
    [Header("=== status")]
    public int maxHP = 500;
    public int attackPower = 5;
    public int defencePower = 4;
    public float moveSpeed = 3.0f;
    public float attackRange = 0.5f;
    public float attackInterval = 5.0f;
    public int currentHP;



    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {

    }

}
