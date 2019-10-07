using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;


public interface ISSActionCallback{
	void SSActionEvent(SSAction source);
}
public class SSAction : ScriptableObject {

	public bool enable = true; 
	public bool destory = false;
	public GameObject gameObject;
	public Transform transform; 
	public ISSActionCallback callback; 
	
	protected SSAction(){} 
	
	public virtual void Start () {
		throw new System.NotImplementedException();
	}
	
	
	public virtual void Update () {
		throw new System.NotImplementedException();
	}
}

public class SSActionManger:MonoBehaviour{
	private Dictionary<int,SSAction> actions = new Dictionary<int, SSAction>();
	private List<SSAction> waitingToAdd = new List<SSAction>();
	private List<int> waitingToDelete = new List<int>();

	protected void Update(){
		foreach (SSAction ac in waitingToAdd){
			actions[ac.GetInstanceID()] = ac;
		}
		waitingToAdd.Clear();
		foreach (KeyValuePair<int,SSAction> kv in actions){
			SSAction ac = kv.Value;
			if(ac.destory){
				waitingToDelete.Add(ac.GetInstanceID());
			}
			else if (ac.enable){
				ac.Update();
			}
		}
		foreach(int key in waitingToDelete){
				SSAction ac = actions[key];
				actions.Remove(key);
				DestroyObject(ac);
		}
		waitingToDelete.Clear();
	}
	
	public void addAction(GameObject gameObject,SSAction action, ISSActionCallback callback){
		action.gameObject = gameObject;
		action.transform = gameObject.transform;
		action.callback = callback;
		waitingToAdd.Add(action);
		action.Start();
	}
}

public class CCMoveToAction:SSAction{
	public Vector3 target;
	public float speed;
	private CCMoveToAction(){}
	public static  CCMoveToAction getAction(Vector3 target, float speed){
		CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
		action.target = target;
		action.speed = speed;
		return action;
	}

	public override void Update(){
		this.transform.position = Vector3.MoveTowards(transform.position,target,speed*Time.deltaTime);
		if(transform.position == target){
			destory = true;
			callback.SSActionEvent(this);
		}
	}

	
	public override void Start(){

	}
}

public class CCSequenceAction:SSAction,ISSActionCallback{
	public List<SSAction> sequence;
	public int repeat = 1;
	public int index = 0;
	public static CCSequenceAction getAction(int repeat,int index,List<SSAction>sequence){
		CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
		action.sequence = sequence;
		action.repeat = repeat;
		action.index = index;
		return action;
	}

	public void SSActionEvent(SSAction source){
		source.destory = false;
		this.index ++;
		if(this.index >= sequence.Count){
			this.index = 0;
			if(repeat>0)
				repeat --;
			if(repeat == 0){
				this.destory = true;
				this.callback.SSActionEvent(this);
			}
		}
	}


	public override void Update(){
        if (sequence.Count == 0) return;
        if (index < sequence.Count){
            sequence[index].Update();
        }
    }

	public override void Start(){
        foreach (SSAction action in sequence){
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this;
            action.Start();
        }
    }
	void OnDestory(){
		foreach(SSAction action in sequence){
			DestroyObject(action);
		}
	}
}

public enum SSActionEventType : int { Started, Competeted }

public class SceneActionManager:SSActionManger,ISSActionCallback{
	public SSActionEventType complete = SSActionEventType.Competeted;
	public CoastModel startCoast;
    public CoastModel endCoast;
    public BoatModel boat;

	Judge judge;

	
	public void getFirstControllerRessoruce(CoastModel start, CoastModel end, BoatModel b){
		startCoast = start;
		endCoast = end;
		boat = b;
		judge = new Judge(startCoast,endCoast,boat);
	}

	public int getGameState(){
		return judge.CheckGameState();
	}

	public void Boatmove(){
		complete = SSActionEventType.Started;
		CCMoveToAction action = CCMoveToAction.getAction(boat.moveBoatPos(),boat.move_speed);
		addAction(boat.getBoat(),action,this);
	}

	public void move(RoleModel role,Vector3 dest){
		
		Vector3 CurrentPos = role.getRolePos();
        Vector3 MiddlePos = CurrentPos;
        if (dest.y > CurrentPos.y){
            MiddlePos.y = dest.y;
        }
        else{
            MiddlePos.x = dest.x;
        }
        SSAction action1 = CCMoveToAction.getAction(MiddlePos, role.move_speed);
        SSAction action2 = CCMoveToAction.getAction(dest, role.move_speed);
        SSAction seqAction = CCSequenceAction.getAction(1, 0, new List<SSAction> { action1, action2 });
        this.addAction(role.getRole(), seqAction, this);
	}

	public void RoleMove(RoleModel role){
		complete = SSActionEventType.Started;
		if(role.isOnBoat()){
            CoastModel coast;
            
            if(boat.getBoatPos() == 1){
                coast = endCoast;
            }
            else
                coast = startCoast;
            boat.rolegetOffBoat(role.getName());
            move(role,coast.getEmptyPos());
            
            role.getOnCoast(coast);
            coast.getOnCoast(role);
        }
        else{
            CoastModel coast = role.getCoast();
            
            if(boat.getEmptyIndex() == -1 || coast.getCoastSign() != boat.getBoatPos())
                return;
            coast.getOffCoast(role.getName());
            
            move(role,boat.getEmptyPos());
            role.getOnboat(boat);
            boat.rolegetOnBoat(role);
        }
    }
	
	public void SSActionEvent(SSAction source){
		complete = SSActionEventType.Competeted;
	}
}