using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    public Slider hpSlider;
    public BossStatus boss;

    void Start()
    {
        hpSlider = GetComponent<Slider>();
        hpSlider.maxValue = boss.maxHP;
        hpSlider.value = boss.maxHP;
    }


    void Update()
    {
        hpSlider.value = boss.currentHP;
    }
}
