using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    #region public 변수
        public string chracter_name = "Follower";
	    public Chracter player_chracter;
    #endregion
    private NavMeshAgent agent;
    private PhotonView photonView;

	void Start()
	{
        photonView = GetComponent<PhotonView>();
        //자기자신이 아닐 때, 하지않는다.
        if (!photonView.isMine)
            return;

        //네비게이션을 끈 후에 , 캐릭터 정보를 생성한다.
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        var child = PhotonNetwork.Instantiate("Chracter/"+chracter_name, new Vector3(50f, 10f, 0f), Quaternion.EulerRotation(new Vector3(15,0,0)), 0);
        child.transform.parent = this.transform;
		child.transform.position = new Vector3(3f, 8f, 0f);
		player_chracter = child.GetComponent<Chracter>();
		transform.position = player_chracter.Find_start_location().transform.position;
        agent.enabled = true;

    }
}
