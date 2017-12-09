using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter : MonoBehaviour {

	
	public string start_location_name;

    private Location start_location;
    
    
    public virtual void Player_Skill()
    {
        Debug.Log("스킬없음");
    }

	public Location Find_start_location()
	{
		return start_location = GameObject.Find (start_location_name).GetComponent<Location> ();
	}


}
