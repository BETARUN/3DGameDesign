using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{

    public int player_turn;
    public int steps;
    private int[,] cells = new int[3, 3];

    // Start is called before the first frame update
    void Start() => restart();

    void restart()//初始化
    {
        player_turn = 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cells[i, j] = 0;
            }
        }
        steps = 0;
    }

    private int Check()//判断胜负
    {
        for (int i = 0; i < 3; i++)//判断纵向三连
        {
            if (cells[i, 0] != 0 && cells[i, 0] == cells[i, 1] && cells[i, 1] == cells[i, 2])
            {
                return cells[i, 0];
            }
        }
        for (int i = 0; i < 3; i++)//判断横向三连
        {
            if (cells[0, i] != 0 && cells[0, i] == cells[1, i] && cells[1, i] == cells[2, i])
            {
                return cells[0, i];
            }
        }
        if (cells[1, 1] != 0 && cells[0, 0] == cells[1, 1] && cells[1, 1] == cells[2, 2] || cells[0, 2] == cells[1, 1] && cells[1, 1] == cells[2, 0])//判断对角线三连
        {
            return cells[1, 1];
        }
        if (steps == 9) return 3;//判断平局
        return 0;
    }

    private void OnGUI()
    {

        if (GUI.Button(new Rect(425, 300, 100, 50), "play_again"))
        {
            restart();
        }

        GUIStyle player1 = new GUIStyle
        {
            fontSize = 35,
            alignment = TextAnchor.MiddleCenter
        };
        GUIStyle player2 = new GUIStyle
        {
            fontSize = 35,
            alignment = TextAnchor.MiddleCenter
        };
        player1.normal.textColor = Color.red;
        player2.normal.textColor = Color.green;

        int result = Check();

        if(result == 1)
        {
            GUI.Label(new Rect(430, 50, 100, 50), "Player1 WIN", style: player1);//玩家1获胜
        }
        if(result == 2)
        {
            GUI.Label(new Rect(430, 50, 100, 50), "Player2 WIN", style: player2);//玩家2获胜
        }
        if(result == 3)
        {
            GUI.Label(new Rect(430, 50, 100, 50), "Dual", style: player1);//平局
        }

        for (int i = 0; i < 3; i++)//下棋时玩家一显示圈，玩家二显示叉
        {
            for (int j = 0; j < 3; j++)
            {
                if (cells[i, j] == 1)
                {
                    GUI.Button(new Rect(400 + i * 50, 100 + j * 50, 50, 50), "◯", style: player1);
                }
                if (cells[i, j] == 2)
                {
                    GUI.Button(new Rect(400 + i * 50, 100 + j * 50, 50, 50), "X", style: player2);
                }
                if (GUI.Button(new Rect(400 + i * 50, 100 + j * 50, 50, 50), ""))
                {
                    if (result == 0)//游戏没有结束，切换玩家
                    {
                        if (player_turn == 1)//轮到玩家1
                        {
                            cells[i, j] = 1;
                        }
                        else//轮到玩家2
                        {
                            cells[i, j] = 2;
                        }
                        steps++;
                        player_turn = -player_turn;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
