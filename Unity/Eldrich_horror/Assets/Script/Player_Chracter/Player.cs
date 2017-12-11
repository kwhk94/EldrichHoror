using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Photon.PunBehaviour, IPunObservable
{

    #region 싱글톤
    public static Player instance;
 
    #endregion
    #region public 변수
    public string chracter_name = "Follower";
	public Chracter player_chracter;
	public int player_number ;
    public string player_name;
	public GameSystem gamesystem;
    public PlayerDataClass playerdata;
    public Location currentLocation;
    public UI_CardSystem cardSystem;
    public NavMeshAgent agent;
    //초기 상태 진입인가?
    public bool inout_bool = true;

    public int currentActionPoint = 2;
    #endregion

    private PhotonView photonView;    
    private GameObject child;
    private bool cardSelectonoff = false;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.isMine)
        {
            instance = this;
        }
    }


    private void Start()
    {
       
        player_number = LobbyPlayerlist.Instance.current_player_number;
        player_name = PhotonNetwork.player.NickName;
        gamesystem = GameSystem.Instance;       
        agent = GetComponent<NavMeshAgent>();
        cardSystem = UI_CardSystem.Instance;
        agent.enabled = false;
        child = GetComponentInChildren<Chracter>().gameObject;
       
        player_chracter = child.GetComponent<Chracter>();
        playerdata = player_chracter.data;

        currentLocation = player_chracter.Find_start_location();
        transform.position = currentLocation.transform.position;
        agent.enabled = true;
        //이름과 번호 동기화
        if(photonView.isMine)
        {
            photonView.RPC("SyncPlayerNameNumber", PhotonTargets.All, player_number, player_name);
        }
    }

    public void LateUpdate()
    {
        //자가 아닌 플레이어는 제외한다.
        if (!photonView.isMine)
        {
            return;
        }

		//게임 현재 플레이어를 확인하여 업데이트한다.
		if (gamesystem.gameRule.current_player == player_number) {
			switch (gamesystem.gameRule.gameOrder) {
			     //액션
			     case Game_order_Name.Action:
                    //카드 선택한 후에만 실행
                        if(cardSystem.cardSelectonoff)
                            ActionFuc();
                        else{
                        //처음 한번만 실행하게 하는 변수값
                            if (inout_bool){
                                inout_bool = false;
                                cardSystem.SetActionCardlist(true);
                            }
                        }
					    break;

				//조우
				case Game_order_Name.Meeting:
					break;
				//신화
				case Game_order_Name.Myth:
					break;
			}
			
		}       
    }
    #region 트리거 함수
    public void OnTriggerEnter(Collider other)
    {
        //자기 이외라면 제외
        if (!photonView.isMine) return;

        //이동시 자기 이외의 장소로 가면 이동 액션을 멈춘다.
        if (GameSystem.Instance.gameRule.gameOrder == Game_order_Name.Action && GameSystem.Instance.gameRule.actionName == Action_Name.Move)
        {
            if (other.GetComponent<Location>() != currentLocation)
            {
                currentLocation.Location_listOnOff(false);
                //다른 장소로 도착시 현재 장소를 다른장소로바꿈               
                EndAction();
                currentLocation = other.GetComponent<Location>();
            }
        }
    }
#endregion

    #region 행동 단계 함수들
    void ActionFuc()
    {
        switch (gamesystem.gameRule.actionName)
        {
            case Action_Name.Deal:
                break;
            case Action_Name.Move:
                MoveFuc();
                break;
            case Action_Name.Rest:
                break;
            case Action_Name.Shop:
                break;
            case Action_Name.Skill:
                break;
            case Action_Name.Ticket:
                break;
        }
    }

    void MoveFuc()
    {
        if (inout_bool)
        {
            inout_bool = false;
            currentLocation.Location_listOnOff(true);         
        }
    }

   

    //액션이 종료시 호출되는 함수
    void EndAction()
    {
        inout_bool = true;
        currentActionPoint -= 1;
        //액션포인트 감소 및 남아있다면 카드를 띄우고 아니라면 다음 사람에게 넘긴다.
        if (currentActionPoint > 0)
        {
            cardSystem.cardSelectonoff = false;
        }
        else
        {
            photonView.RPC("ChangeChracternumber", PhotonTargets.MasterClient,Game_order_Name.Action);
        }
    }

    #endregion

    #region 동기화 함수
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            playerdata.SerializeSend(stream, info);
        }
        else
        {
            playerdata.SerializeReceive(stream, info);
        }
    }
#endregion

    #region RPC 함수
    [PunRPC]
    void ChangeChracternumber(Game_order_Name name)
    {        
        currentActionPoint = 2;
        GameSystem.Instance.gameRule.current_player = (GameSystem.Instance.gameRule.current_player+1)%PhotonNetwork.playerList.Length;
        cardSystem.cardSelectonoff = false;
        if (GameSystem.Instance.gameRule.current_player == GameSystem.Instance.gameRule.start_player)
        {
            GameSystem.Instance.gameRule.gameOrder = name;
        }
    }
    [PunRPC]
    void SyncPlayerNameNumber(int num, string name)
    {
        this.player_number = num;
        this.player_name = name;
    }
    #endregion



}

[System.Serializable]
public class PlayerDataClass
{
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

    public void SerializeSend(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(hp);
        stream.SendNext(mp);
        stream.SendNext(power);
    }
    public void SerializeReceive(PhotonStream stream, PhotonMessageInfo info)
    {
        hp = (int)stream.ReceiveNext();
        mp = (int)stream.ReceiveNext();
        power = (int)stream.ReceiveNext();
    }
    
}
