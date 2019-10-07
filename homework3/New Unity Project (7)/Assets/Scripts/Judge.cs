using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame;

public class Judge : MonoBehaviour {
	public CoastModel startCoast;
    public CoastModel endCoast;
    public BoatModel boat;

	//实例化的同时同步对象
	public Judge(CoastModel start, CoastModel end, BoatModel b){
		startCoast = start;
		endCoast = end;
		boat = b;
	}

	// Use this for initialization
	public int CheckGameState(){
        int start_priest = (startCoast.getRoleNumber())[0];
        int start_devil = (startCoast.getRoleNumber())[1];
        int end_priest = (endCoast.getRoleNumber())[0];
        int end_devil = (endCoast.getRoleNumber())[1];

        if(end_priest == 3 && end_devil == 3)
            return 2;
        int [] boat_roles = boat.getRoleNumber();
        //船在开始岸
        if(boat.getBoatPos() == 0){
            start_priest += boat_roles[0];
            start_devil += boat_roles[1];
        }
        else{
            end_priest += boat_roles[0];
            end_devil += boat_roles[1];
        }

        if((start_priest > 0 && start_priest < start_devil) || (end_priest>0&&end_priest<end_devil))
            return 1;
        return 0;
    }
}
