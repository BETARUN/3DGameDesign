using UnityEngine;
using UnityEngine.UI;

public class IMGUIHealthControl : MonoBehaviour {
    // 当前血量
    public float health = 50;
    // 增/减后血量
    private float resultHealth = 0;

    private Rect bloodBar;
    private Rect upButton;
    private Rect downButton;

	void Start ()
    {
        Debug.Log("smg");
        // 初始值
        health = 50;
        resultHealth = 50;
        // 居中
        float w = Screen.width;
        float h = Screen.height;
        // 血条横向
        bloodBar = new Rect((w - 200) / 2, (h - 20) / 2, 200, 20);
        // 加血按钮
        upButton = new Rect(w / 3, 100, 40, 20);
        // 减血按钮
        downButton = new Rect(w / 3 * 2, 100, 40, 20);
	}

	void OnGUI ()
    {
        Debug.Log("smg");
        if (GUI.Button(upButton, "加血"))
            resultHealth = health + 10 > 100 ? 100 : health + 10;
        if (GUI.Button(downButton, "减血"))
            resultHealth = health - 10 < 0.1 ? 0 : health - 10;
        GUI.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        health = Mathf.Lerp(health, resultHealth, 0.05f);
        GUI.HorizontalScrollbar(bloodBar, 0, health, 0, 100);
    }
}