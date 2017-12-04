﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter : MonoBehaviour {

	public Location start_location;
	public Material chracter_mat;
	public string start_location_name;


	public void Awake()
	{
		GetComponent<MeshRenderer>().material = chracter_mat;
		Find_start_location (start_location_name);
	}


    public virtual void Player_Skill()
    {
        Debug.Log("스킬없음");
    }

	public void Find_start_location(string name)
	{
		start_location = GameObject.Find (name).GetComponent<Location> ();
	}


}