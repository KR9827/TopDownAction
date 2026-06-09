using UnityEngine;

public class EnemyStatus2 : MonoBehaviour
{
    [Header("=== Status")]
    public int maxHP = 20;
    public int currentHP;
    public int attackPower = 2;
    public int defencePower = 2;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        
    }
}
