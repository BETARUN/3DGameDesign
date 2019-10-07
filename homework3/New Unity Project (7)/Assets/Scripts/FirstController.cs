using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;


//创建场记
public class FirstController:MonoBehaviour, ISceneController, UserAction{
    public CoastModel startCoast;
    public CoastModel endCoast;
    public BoatModel boat;
    public RoleModel[] roles;
    InterGUI inter_gui;

    public SceneActionManager actionManager; //场景动作管理

    Vector3 water_pos = new Vector3(0,0.2F,0);

    void Awake(){
        SSDirector director = SSDirector.getInstance();
        //Debug.Log("222");
        director.currentScenceController = this;
        inter_gui = gameObject.AddComponent<InterGUI>() as InterGUI;
        LoadResources();
        actionManager = gameObject.AddComponent<SceneActionManager>() as SceneActionManager;
        actionManager.getFirstControllerRessoruce(startCoast,endCoast,boat);//对象同步
    }
 

    public void LoadResources(){
        //创建对象
        startCoast = new CoastModel("from");
        endCoast = new CoastModel("to");
        boat = new BoatModel();
        roles = new RoleModel[6];
        GameObject water = Instantiate(Resources.Load("Perfabs/water",typeof(GameObject)),water_pos,Quaternion.identity,null) as GameObject;
        water.name = "water";
        for(int i = 0;i < 3;i ++){
            RoleModel priest = new RoleModel("priest");
            priest.setName("priest" + i);
            priest.setPos(startCoast.getEmptyPos());
            priest.getOnCoast(startCoast);
            startCoast.getOnCoast(priest);
            roles[i] = priest;
        }

        for(int i = 0;i < 3;i ++){
            RoleModel devil = new RoleModel("devil");
            devil.setName("devil" + i);
            devil.setPos(startCoast.getEmptyPos());
            devil.getOnCoast(startCoast);
            startCoast.getOnCoast(devil);
            roles[i + 3] = devil;
        }
    } 

    public void moveBoat(){
        if(boat.isEmpty() || inter_gui.state != 0)
            return;
        actionManager.Boatmove();//改成用actionManager控制
        inter_gui.state = actionManager.getGameState();
    }

    public void moveRole(RoleModel role){
        if(inter_gui.state != 0) 
            return;
            //role在船上
        actionManager.RoleMove(role);
        inter_gui.state = actionManager.getGameState();
    }

    public void reStart(){
        startCoast.Reset();
        endCoast.Reset();
        //Debug.Log(boat.getBoatPos());
        boat.Reset();
        //boat.moveBoat();
        for(int i = 0;i < roles.Length;i ++){
            roles[i].Reset();
        }
    }

}