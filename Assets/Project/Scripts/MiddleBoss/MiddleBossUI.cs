using UnityEngine;
using UnityEngine.UI;

public class MiddleBossUI : MonoBehaviour
{
    public Slider hpSlider;
    MiddleBossStatus status;

    void Start()
    {
        status = GetComponentInParent<MiddleBossStatus>();

        if (status != null)
        {
            hpSlider.maxValue = status.maxHP;
            hpSlider.value = status.maxHP;
        }
    }

    void Update()
    {
        if (status != null)
            hpSlider.value = status.currentHP;
    }
}
