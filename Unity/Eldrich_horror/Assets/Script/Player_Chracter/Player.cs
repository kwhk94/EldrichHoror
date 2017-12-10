using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Photon.PunBehaviour, IPunObservable
{

    public static Player instance;
    #region public 변수
    public string chracter_name = "Follower";
	public Chracter player_chracter;

    public int m_hp = 10;

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

    public void Update()
    {
        if (!photonView.isMine)
            return;

       
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(m_hp);
            //Debug.Log("hp 정보 보내기 : " + m_hp + photonView.owner);
        }
        else
        {
            m_hp = (int)stream.ReceiveNext();
            //Debug.Log("hp 정보 변환 : " + m_hp+ photonView.owner);
        }
    }


    #region RPC 함수들

    #endregion
}
