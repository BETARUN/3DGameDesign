using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;

public class ClickGUI : MonoBehaviour
{
    private UserAction action;
    
    public RoleModel role;//点击Role
    public BoatModel boat;//点击Boat
    public void setBoatModel(BoatModel obj){
        boat = obj;
    }
    public void setRoleModel(RoleModel obj){
        role = obj;
    }

    void Awake(){
        action = SSDirector.getInstance().currentScenceController as UserAction;
    }

    void OnMouseDown(){
        //if (action == null)
          //  Debug.Log(role.getName());
        if(role == null && boat == null)
            return;
        if(boat != null){
            action.moveBoat();
            return;
        }
        if(role != null){
            action.moveRole(role);
            return;
        }
        
    }
}
