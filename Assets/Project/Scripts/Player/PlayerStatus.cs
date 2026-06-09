using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("===status")]
    public int maxHP = 100;
    public int currentHP;
    public int attackPower = 5;
    public int defencePower = 2;



    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        
    }
}
