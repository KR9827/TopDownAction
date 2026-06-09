using UnityEngine;

public class MiddleBossStatus : MonoBehaviour
{
    [Header("=== Status")]
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 3;
    public int defencePower = 2;
    public float moveSpeed = 2.0f;
    public float attackRange = 5.0f;
    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        
    }
}
