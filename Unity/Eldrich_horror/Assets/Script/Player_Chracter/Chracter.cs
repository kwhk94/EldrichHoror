using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chracter : MonoBehaviour {

	public Location start_location;
	public string start_location_name;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        //자기자신이 아닐 때, 하지않는다.
        if (!photonView.isMine)
            return;
    }


    public virtual void Player_Skill()
    {
        Debug.Log("스킬없음");
    }

	public Location Find_start_location()
	{
		return start_location = GameObject.Find (start_location_name).GetComponent<Location> ();
	}


}
