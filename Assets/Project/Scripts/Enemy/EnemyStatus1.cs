using UnityEngine;

public class EnemyStatus1 : MonoBehaviour
{
    [Header("=== Status")]
    public int maxHP = 15;
    public int currentHP;
    public int attackPower = 2;
    public int defencePower = 1;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        
    }
}
