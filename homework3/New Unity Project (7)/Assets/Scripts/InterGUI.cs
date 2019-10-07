using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;
public class InterGUI : MonoBehaviour
{
    private UserAction action;
    public int state = 0;//0表示重新开始，1失败，2获胜
    // Start is called before the first frame update
    public int time;//剩余时间
    void Awake()
    {
        action = SSDirector.getInstance().currentScenceController as UserAction;
        time = 100;
    }

    void FixedUpdate(){
        Time.fixedDeltaTime = 1;
        time --;
    }

    void OnGUI(){
        GUIStyle style = new GUIStyle();
        GUIStyle timeStyle = new GUIStyle();
        GUIStyle buttonStyle = new GUIStyle();

        timeStyle.fontSize = 40;
        timeStyle.normal.textColor = Color.black;
        style.fontSize = 60;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.red;
        buttonStyle = new GUIStyle("Button");
        buttonStyle.fontSize = 40;
        if (state == 0){
            GUI.Label(new Rect(Screen.width / 2 + 200, Screen.height / 2 - 400, 100, 50), "TimeLeft: " + time, timeStyle);
            if (time == 0) 
                state = 1;//时间耗尽
        }
        else if(state == 1){
            //System.Threading.Thread.Sleep(200);//延时使游戏更顺畅
            GUI.Label(new Rect(Screen.width/2-50,Screen.height/2-200,100,50),"GameOver!",style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2-100, 140, 70), "Restart",buttonStyle)){
                state = 0;
                time = 100;
                action.reStart();
            }
        }
        else if(state == 2){
            //System.Threading.Thread.Sleep(200);
            GUI.Label(new Rect(Screen.width/2-50,Screen.height/2-200,100,50),"You Win!",style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2-100, 140, 70), "Restart",buttonStyle)){
                state = 0;
                time = 100;
                action.reStart();
            }
        }
    }
}
