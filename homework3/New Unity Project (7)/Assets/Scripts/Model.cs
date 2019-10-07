using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mygame{
        //用户操作
    public interface UserAction{
        void moveBoat();//移动船
        void moveRole(RoleModel role);//移动对象
        void reStart();//重启游戏
        //int CheckGameState();//检查游戏是否结束
    }
    

    public interface ISceneController{
        void LoadResources();//加载场景
    }

    public class SSDirector : System.Object{
        private static SSDirector _instance;
        public ISceneController currentScenceController { get; set; }
        public static SSDirector getInstance()
        {
            if (_instance == null)
            {
                _instance = new SSDirector();
            }
            return _instance;
        }
    }

    
    public class BoatModel{
        public GameObject boat;
        //Move move;
        ClickGUI click;
        Vector3 [] fromPos;//开始位置
        Vector3 [] toPos;//结束位置
        RoleModel[] roles = new RoleModel[2];//船上最多两个角色
        int boat_pos=0;//开始位置0，结束位置1
        public int move_speed = 20;//新增移动速度
        public BoatModel(){
            boat_pos = 0;
            boat = Object.Instantiate(Resources.Load("Perfabs/boat", typeof(GameObject)), new Vector3(4,0.8F,0), Quaternion.identity, null) as GameObject;
            boat.transform.Rotate(0, 90, 0, Space.Self);
            fromPos = new Vector3[]{new Vector3(3.5F,1.2F,0), new Vector3(4.5F,1.2F,0)};
            toPos = new Vector3[]{new Vector3(-4.5F,1.2F,0),new Vector3(-3.5F,1.2F,0)};
            //move = boat.AddComponent(typeof(Move)) as Move;
            click = boat.AddComponent(typeof(ClickGUI)) as ClickGUI;
            click.setBoatModel(this);
        }


        //新版本改变
        public Vector3 moveBoatPos(){
            if(boat_pos == 0){
                //move.MoveTo(new Vector3(-3F,0.8F,0));
                boat_pos = 1;
                return new Vector3(-3F,0.8F,0);
            }
            else if(boat_pos == 1){
                //move.MoveTo(new Vector3(4,0.8F,0));
                boat_pos = 0;
                return new Vector3(4F,0.8F,0);
            }
            return new Vector3();
        }

        public bool isEmpty(){
            for(int i = 0;i < roles.Length;i ++){
                if(roles[i] != null)
                    return false;
            }
            return true;
        }

        public int getEmptyIndex(){
            for(int i = 0;i < roles.Length;i ++){
                if(roles[i] == null)
                    return i;
            }
            return -1;
        }

        public Vector3 getEmptyPos(){
            if(boat_pos == 0){
                return fromPos[getEmptyIndex()];
            }
            else{
                return toPos[getEmptyIndex()];
            }
        }

        public void rolegetOnBoat(RoleModel role){
            roles[getEmptyIndex()] = role;
        }

        public RoleModel rolegetOffBoat(string name){
            for (int i = 0; i < roles.Length; i++) {
				if (roles[i] != null && roles[i].getName() == name) {
					RoleModel temp = roles[i];
					roles[i] = null;
					return temp;
				}
			}
			return null;
        }

        public int getBoatPos(){
            return boat_pos;
        }
        public GameObject getBoat(){
            return boat;
        }

        public void Reset(){
            boat.transform.position = new Vector3(4,0.8F,0);//结束时在对岸移回
            roles = new RoleModel[2];
            boat_pos = 0;
            //move.reset();
        }

        public int[]getRoleNumber(){
            int[] count = {0,0};
            for(int i = 0;i < roles.Length; i ++){
                if(roles[i] != null){
                    if(roles[i].getType() == 0){
                        count[0] ++;
                    }
                    else
                        count[1] ++;
                }
            }
            return count;
        }
    }


    public class RoleModel{
        GameObject role {get;set;}
        int roleType;//0为牧师1为魔鬼
        //Move move;
        ClickGUI clickgui;
        CoastModel coast;
        bool onBoat = false;

        //新增速度
        public int move_speed = 20;

        public GameObject getRole(){
            return role;
        }

        public RoleModel(string type){
            if(type == "priest"){
                role = Object.Instantiate(Resources.Load("Perfabs/priest1", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                role.transform.Rotate(0, 190, 0, Space.Self);
                roleType = 0;
            }
            else if(type == "devil"){
                role = Object.Instantiate(Resources.Load("Perfabs/devil1", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                role.transform.Rotate(0, 240, 0, Space.Self);
                roleType = 1;
            }
            else{
                Debug.Log("Error Type");
            }
            //move = role.AddComponent(typeof(Move)) as Move;
            clickgui = role.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickgui.role = this;
        }

        //新增
        public Vector3 getRolePos(){
            return role.transform.position;
        }

        public string getName(){
            return role.name;
        }

        public void setName(string _name){
            role.name = _name;
        }

        public void setPos(Vector3 v){
            role.transform.position = v;
        }

        public int getType(){
            return roleType;
        }

        public bool isOnBoat(){
            return onBoat;
        }

        public CoastModel getCoast(){
            return coast;
        }

        public void getOnboat(BoatModel boat){
            coast = null;
            role.transform.parent = boat.getBoat().transform;
            onBoat = true;
        }

        public void getOnCoast(CoastModel coastmodel){
            coast = coastmodel;
            role.transform.parent = null;
            onBoat = false;
        }

        public void Reset(){
            //move.reset();
            coast = (SSDirector.getInstance().currentScenceController as FirstController).startCoast;
            getOnCoast(coast);
            setPos(coast.getEmptyPos());
            coast.getOnCoast(this);
        }
    }

    public class CoastModel{
        GameObject coast;
        Vector3[] positions;
        int coastSign;//开始陆地0，结束陆地1
        RoleModel[] roles = new RoleModel[6];//3个魔鬼3个牧师
        public CoastModel(string state){
            positions = new Vector3[] {new Vector3(5.5F,1.78F,0), new Vector3(6.5F,1.78F,0), new Vector3(7.5F,1.78F,0), 
            new Vector3(8.6F,1.78F,0), new Vector3(9.6F,1.78F,0), new Vector3(10.6F,1.8F,0)};

            if(state == "from"){
                coast = Object.Instantiate (Resources.Load ("Perfabs/coast", typeof(GameObject)), new Vector3(8,0.7F,0), Quaternion.identity, null) as GameObject;
                coast.name = "from";
                coastSign = 0;
            }
            else if(state == "to"){
                coast = Object.Instantiate (Resources.Load ("Perfabs/coast", typeof(GameObject)), new Vector3(-8,0.7F,0), Quaternion.identity, null) as GameObject;
                coast.name = "to";
                coastSign = 1;
            }
            else{
                Debug.Log("Coast state error");
            }

        }

        public int getEmptyIndex(){
            for(int i = 0;i < roles.Length;i ++){
                if(roles[i] == null)
                    return i;
            }
            return -1;
        }

        public Vector3 getEmptyPos(){
            // Debug.Log(getEmptyIndex());
            Vector3 pos = positions[getEmptyIndex()];
            if(coastSign == 1)
                pos.x = -pos.x;
            return pos;
        }

        public int getCoastSign(){
            return coastSign;
        }

        public void getOnCoast(RoleModel role){
            roles[getEmptyIndex()] = role;
        }

       public RoleModel getOffCoast(string roleName){
           for(int i = 0;i < roles.Length;i ++){
               if(roles[i] != null && roles[i].getName() == roleName){
                   RoleModel tmp = roles[i];
                   roles[i] = null;
                   return tmp;
               }
           }
           return null;
       } 

        public int [] getRoleNumber(){
            int[] count = {0,0};
            for(int i = 0;i < roles.Length; i ++){
                if(roles[i] != null){
                    if(roles[i].getType() == 0){
                        count[0] ++;
                    }
                    else
                        count[1] ++;
                }
            }
            return count;

        }

        public void Reset(){
            roles = new RoleModel[6];
        }

    }
}
