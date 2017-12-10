using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public enum Game_order_Name { Action = 0x1, Meeting = 0x2, Myth = 0x4 };
//행동 단계에서의 행동들, 이동, 휴식, 자산획득, 이동준비,거래 , 구성물 행동
[SerializeField]
public enum Action_Name { Move = 0x1, Rest = 0x2, Shop = 0x4, Ticket = 0x8, Deal = 0x10, Skill = 0x20 };



public class GameSystem : Photon.PunBehaviour, IPunObservable
{
    #region 싱글톤
    private static GameSystem instance;
    public static GameSystem Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<GameSystem>();
            if (!instance)
                Debug.Log("GameSystem instance not find");
            return instance;
        }

        set { instance = value; }
    }
    #endregion
    #region public변수
    public UI_moveicon ruin_icon;
    public UI_moveicon symptom_icon;
    public delegate void UI_event();
    public event UI_event UI_eventlist;

    //현재 단계
    public Text game_Order_Text;
	public LobbyPlayerlist player_list_class;
	public GameRuleSerealize gameRule = new GameRuleSerealize();
   
    #endregion

    #region private 변수
    private PhotonView photonView;

    //게임의 정보를 저장한 클래스
    


    #endregion

    private void Start()
    {
        //포톤게임메니져(로비메니져)에서 지웠던 포톤 뷰를 다시 만든다.
        //씬이 바뀔 때마다 포톤 뷰의 ID값이 갱신되는데, 
        //이전 포톤뷰가 안지워지면 ID가 중복되어 지우고 다시만든다.
        PhotonGameManager.Instance.MakePhotonView();
		player_list_class = LobbyPlayerlist.Instance;
        

        photonView = GetComponent<PhotonView>();
        
        //플레이어 프리팹 생성
        GameObject temp_player = PhotonNetwork.Instantiate("Chracter/" + LobbyPlayerlist.Instance.Charcter_list[LobbyPlayerlist.Instance.current_chracter_number].name
            , new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		GameObject player_UI = PhotonNetwork.Instantiate("UI/" + "ChracterUI"
			, new Vector3(91f, 610f, 0f), Quaternion.Euler(0,0,180), 0);   
        Debug.Log("캐릭터 이름 :" + temp_player.GetComponent<Player>().chracter_name);
 
    }


    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            gameRule.SereializeSend(stream, info);
            game_Order_Text.text = gameRule.gameOrder.ToString() + "\n" + gameRule.actionName.ToString();
            
        }
        else
        {
            gameRule.SereializeReceive(stream, info);
            game_Order_Text.text = gameRule.gameOrder.ToString() + "\n" + gameRule.actionName.ToString();
        }
        
    }

    public void LateUpdate()
    {
        //멸망 토큰의 이동 애니메이션
        if (ruin_icon.currentnum!=ruin_icon.targetnum)
        {
            ruin_icon.move_icon();
        }
        //징조 토큰 이동 애니메이션
        if(symptom_icon.currentnum != symptom_icon.targetnum)
        {
            symptom_icon.move_icon();
        }
        //상단 ui 에니메이션
        if(UI_eventlist != null)
            UI_eventlist();      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            gameRule.gameOrder = 
                (gameRule.gameOrder == Game_order_Name.Myth) ? Game_order_Name.Action : Game_order_Name.Myth;
            //photonView.RPC("Changecurrent_order", PhotonTargets.All, current_order);
        }
    }
}

#region 직렬화한 클래스

[System.Serializable]
public class GameRuleSerealize
{
    //행동, 조우,신화 단계
    public Game_order_Name gameOrder = Game_order_Name.Action;
    //액션 단계 이름
    public Action_Name actionName = Action_Name.Deal;

	public int current_player= 0;
	public int start_player = 0;

    #region Sereialize 보내기 받기
    public void SereializeSend(PhotonStream stream, PhotonMessageInfo info)
    {
        stream.SendNext(this.gameOrder);
        stream.SendNext(this.actionName);
    }
    public void SereializeReceive(PhotonStream stream, PhotonMessageInfo info)
    {
        this.gameOrder = (Game_order_Name)stream.ReceiveNext();
        this.actionName = (Action_Name)stream.ReceiveNext();
    }
#endregion

}
#endregion