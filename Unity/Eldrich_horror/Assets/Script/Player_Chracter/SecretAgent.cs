using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAgent : Chracter {


	void Awake()
	{
		start_location_name = "Location_Sydney";
		Find_start_location (start_location_name);
	}
}
