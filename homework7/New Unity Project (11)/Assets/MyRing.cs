using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRing : MonoBehaviour
{
    public ParticleSystem _particleSystem;      // 粒子系统对象。
    public int maxParticleNumber = 10000;       // 发射的最大粒子数
    public float pingPong = 0.05f;              // 游离范围
    public float particleSize = 0.05f;          // 粒子的大小
    public float maxRadius = 10, minRadius = 5; // 粒子旋转半径范围
    public float particleSpeed = 0.05f;         // 粒子运动的速度

    private float[] particleAngles;         // 每个粒子的位置偏移角
    private float[] particleRadius;         // 每个粒子的运动半径
    private float time = 0;
    private ParticleSystem.Particle[] particleArray;
    public Color[] colors = { new Color(255, 255, 255), new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 255, 0), new Color(0, 255, 255), new Color(255, 0, 255), new Color(0, 0, 0) };
    private float colorTimeOut = 0;
    private Gradient gradient;              // 颜色控制器
    private GradientAlphaKey[] alphaKeys;
    private GradientColorKey[] colorKeys;

    // Use this for initialization
    void Start()
    {
        // 初始化粒子数组、粒子角度数组、粒子半径数组
        _particleSystem = GetComponent<ParticleSystem>();
        particleArray = new ParticleSystem.Particle[maxParticleNumber];
        particleAngles = new float[maxParticleNumber];
        particleRadius = new float[maxParticleNumber];

        // 初始化粒子系统
        _particleSystem.startSpeed = 0;
        _particleSystem.loop = false;
        _particleSystem.maxParticles = maxParticleNumber;
        _particleSystem.Emit(maxParticleNumber);            // 发射粒子
        _particleSystem.GetParticles(particleArray);

        // 初始化各粒子位置
        for (int i = 0; i < maxParticleNumber; i++)
        {
            // 随机生成每个粒子距离中心的半径，同时希望粒子集中于平均半径附近
            float midRadius = (maxRadius + minRadius) / 2;
            float rate1 = Random.Range(1.0f, midRadius / minRadius);
            float rate2 = Random.Range(midRadius / maxRadius, 1.0f);
            particleRadius[i] = Random.Range(minRadius * rate1, maxRadius * rate2);
            // 设置粒子大小
            particleArray[i].startSize = particleSize;
            // 随机生成每个粒子的角度
            particleAngles[i] = Random.Range(0, 360);
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0); // 放置粒子
        }

        _particleSystem.SetParticles(particleArray, particleArray.Length);

        // 初始化梯度颜色控制器
        alphaKeys = new GradientAlphaKey[5];
        alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
        alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
        alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
        alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
        alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.6f;
        colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0; colorKeys[0].color = Color.white;
        colorKeys[1].time = 1; colorKeys[1].color = Color.white;
        gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
    }

    // Update is called once per frame
    void Update()
    {
        colorTimeOut += Time.deltaTime;
        for (int i = 0; i < particleArray.Length; i++)
        {
            time += Time.deltaTime;
            // 更改粒子角度(一半在逆时针，一半在顺时针，而且速度有10个层级)
            particleAngles[i] = (particleAngles[i] + (i % 10 + 1) * (i % 2 == 0 ? particleSpeed : 0 - particleSpeed) + 360) % 360;
            // 更改粒子半径(造成粒子波动效果，而且波动是在一定范围内的)
            particleRadius[i] += (Mathf.PingPong(time / minRadius / maxRadius, pingPong) - pingPong / 2);
            // 更改粒子颜色
            // particleArray[i].startColor = colors[(int)colorTimeOut % colors.Length];
            colorKeys[0].time = 0;
            colorKeys[1].time = 1;
            colorKeys[0].color = colors[(int)colorTimeOut % colors.Length];
            colorKeys[1].color = colors[(int)colorTimeOut % colors.Length];
            gradient.SetKeys(colorKeys, alphaKeys);
            particleArray[i].startColor = gradient.Evaluate(particleAngles[i] / 360.0f);
            // 设置粒子位置
            float rad = particleAngles[i] / 180 * Mathf.PI;
            particleArray[i].position = new Vector3(particleRadius[i] * Mathf.Cos(rad), particleRadius[i] * Mathf.Sin(rad), 0);
        }
        _particleSystem.SetParticles(particleArray, particleArray.Length);
    }
}
