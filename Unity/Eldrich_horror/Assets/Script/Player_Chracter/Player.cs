using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    public static Player instance;
    #region public 변수
    public string chracter_name = "Follower";
	public Chracter player_chracter;
    #endregion
    private NavMeshAgent agent;
    private PhotonView photonView;    
    private GameObject child;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        if (photonView.isMine)
        {
            instance = this;
           
        }

        agent.enabled = false;
        child = GetComponentInChildren<Chracter>().gameObject;
        player_chracter = child.GetComponent<Chracter>();
        transform.position = player_chracter.Find_start_location().transform.position;
        agent.enabled = true;
    }
  
    #region RPC 함수들

    #endregion
}
