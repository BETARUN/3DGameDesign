using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START, FUNISH}
public enum ActionMode { NOTSET, PHYSIC, KINEMATIC }

public interface IUserAction{
    ActionMode getActionMode();
    void setActionMode(ActionMode mode);
    GameState getGameState();
    void setGameState(GameState gameState);
    int getScore();
    void hit(Vector3 pos);
}
