using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    public Slider hpSlider;
    EnemyStatus1 status1;
    EnemyStatus2 status2;

    void Start()
    {
        status1 = GetComponentInParent<EnemyStatus1>();
        status2 = GetComponentInParent<EnemyStatus2>();

        if (status1 != null)
        {
            hpSlider.maxValue = status1.maxHP;
            hpSlider.value = status1.maxHP;
        }
        else if (status2 != null)
        {
            hpSlider.maxValue = status2.maxHP;
            hpSlider.value = status2.maxHP;
        }
    }

    void Update()
    {
        if (status1 != null)
            hpSlider.value = status1.currentHP;
        else if (status2 != null)
            hpSlider.value = status2.currentHP;
    }
}
