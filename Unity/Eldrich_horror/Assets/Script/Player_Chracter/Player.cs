using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public string chracter_name = "Follower";
	public Chracter player_chracter;
	public GameObject chracter_object;

	void Start()
	{
		var child = Instantiate (chracter_object);
		child.transform.parent = this.transform;
		child.transform.position = this.transform.position;
		player_chracter = child.GetComponent<Chracter>();
		transform.position = player_chracter.start_location.transform.position;

	}
}
