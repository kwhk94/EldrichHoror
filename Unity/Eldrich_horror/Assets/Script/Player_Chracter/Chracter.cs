using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter : MonoBehaviour {

	
	public string start_location_name;
    //체력
    public int hp;
    //마력
    public int mp;
    //힘
    public int power;
    //지식
    public int knowledge;
    //영향력
    public int Influence;
    //관찰력
    public int observation;
    //의지
    public int will;


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
