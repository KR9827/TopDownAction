using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    public Slider hpSlider;
    public PlayerStatus player;

    void Start()
    {
        hpSlider.maxValue = player.maxHP;
        hpSlider.value = player.maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = player.currentHP;
    }
}
