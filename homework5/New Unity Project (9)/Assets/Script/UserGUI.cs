using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGUI : MonoBehaviour {
    private IUserAction action;
    bool isFirst = true;
    GUIStyle red;
    GUIStyle black;
    bool pause_flag = true;
    void Start () {
        action = Director.getInstance().current as IUserAction;
        black = new GUIStyle("button");
        black.fontSize = 20;
        red = new GUIStyle();
        red.fontSize = 30;
        red.fontStyle = FontStyle.Bold;
        red.normal.textColor = Color.red;
        red.alignment = TextAnchor.UpperCenter;
    }

    private void OnGUI()
    {
        if (action.getGameState() == GameState.FUNISH)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 150, 200, 100), action.getScore() >= 30 ? "你胜利了" : "你失败了", red);
            if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 50, 120, 40), "重新开始", black))
            {
                SceneManager.LoadScene("DiskAttack");
            }
            return;
        }
        Rect rect = new Rect(Screen.width / 2 - 100, 0, 200, 40);
        Rect rect2 = new Rect(Screen.width / 2 - 60, 60, 120, 40);

        if (Input.GetButtonDown("Fire1") && action.getGameState() != GameState.PAUSE)
        {
            Vector3 pos = Input.mousePosition;
            action.hit(pos);
        }

        if (!isFirst)
        {
            GUI.Label(rect, "你的分数: " + action.getScore().ToString(), red);
        }
        else
        {
            GUIStyle blackLabel = new GUIStyle();
            blackLabel.fontSize = 16;
            blackLabel.normal.textColor = Color.black;
            GUI.Label(new Rect(Screen.width / 2 - 250, 120, 500, 200), "一共3个关卡，每一个关卡有10个飞碟，飞碟" +
                "的颜色是不一样的\n如果攻击白色的飞碟，你会得到1分。如果攻击灰色的飞碟\n" +
                "则会得到2分，如果攻击黑色的飞碟（速度最快），你会得到4分。\n" +
                "游戏结束时得到超过30分就胜利了！", blackLabel);
        }

        if (pause_flag)
        {
            if (action.getGameState() == GameState.RUNNING && GUI.Button(rect2, "暂停", black))
            {
                action.setGameState(GameState.PAUSE);
            }
            else if (action.getGameState() == GameState.PAUSE && GUI.Button(rect2, "继续", black))
            {
                action.setGameState(GameState.RUNNING);
            }
        }

        if(action.getActionMode() == ActionMode.NOTSET)
        {
            if(GUI.Button(new Rect(Screen.width / 2 - 60, 0, 120, 40), "物理模式", black))
            {
                action.setActionMode(ActionMode.PHYSIC);
                pause_flag = false;
            }
            if (GUI.Button(rect2, "运动学模式", black))
            {
                action.setActionMode(ActionMode.KINEMATIC);
            }
        }
        else if (isFirst && GUI.Button(rect2, "开始", black))
        {
            isFirst = false;
            action.setGameState(GameState.ROUND_START);
        }

        if(!isFirst && action.getGameState() == GameState.ROUND_FINISH && GUI.Button(rect2, "下一关卡", black))
        {
            action.setGameState(GameState.ROUND_START);
        }
    }
}
