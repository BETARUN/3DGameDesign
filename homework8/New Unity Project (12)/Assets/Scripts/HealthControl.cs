using UnityEngine;
using UnityEngine.UI;

public class HealthControl : MonoBehaviour
{
    // 血条
    public Slider mainSlider;
    // 恢复血的时间的间隔
    public float duration = 10;
    // 用于计时
    private float dur_time = 0;
    // 当前血量
    private float health;
    // 增/减后血量
    private float resultHealth;
    // 增减血
    private bool flag = false;

    void Start()
    {
        // 设置最大血量、最小血量、回血间隔以及现在血量
        mainSlider.maxValue = 100;
        mainSlider.minValue = 0;
        duration = 10;
        // mainSlider.wholeNumbers = true;
        mainSlider.value = mainSlider.maxValue / 2;
        health = mainSlider.value;
        resultHealth = health;
    }

    void Update()
    {
        // 计时以回血
        if (mainSlider.value < mainSlider.maxValue - 0.1 && !flag)
        {
            dur_time += Time.deltaTime;
            if (dur_time >= duration)
            {
                resultHealth = mainSlider.value + 10 > mainSlider.maxValue ? mainSlider.maxValue : mainSlider.value + 10;
                dur_time = 0;
            }
        }
        // 发生碰撞，减血
        else if (flag)
        {
            resultHealth = mainSlider.value - 20 < mainSlider.minValue ? mainSlider.minValue : mainSlider.value - 20;
            flag = false;
        }
        // 平滑减少血量
        health = Mathf.Lerp(health, resultHealth, 0.05f);
        mainSlider.value = health;
        // 控制slider中Fill Area的显示，以更符合血量的显示
        if (mainSlider.value <= 0.01)
            mainSlider.transform.GetChild(1).localScale = Vector3.zero;
        else
            mainSlider.transform.GetChild(1).localScale = Vector3.one;
    }

    // 外部控制减少血量
    public void ReduceHealth()
    {
        flag = true;
    }
}