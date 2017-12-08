using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
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
    //행동, 조우,신화 단계
    public enum Game_order_Name {Action =0x1, Meeting = 0x2, Myth = 0x4 };
	//행동 단계에서의 행동들, 이동, 휴식, 자산획득, 이동준비,거래 , 구성물 행동
	public enum Action_Name { Move = 0x1, Rest = 0x2, Shop = 0x4 ,Ticket = 0x8, Deal = 0x10, Skill = 0x20 };
	//현재 단계

    [SerializeField]
	public Game_order_Name current_order = Game_order_Name.Myth;
    //플레이어 프리팹
    public GameObject player_Prefeb;
    #endregion

    #region private 변수
    private PhotonView photonView;
    #endregion

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        //자기자신이 아닐 때, 하지않는다.
        if (!photonView.isMine)
            return;

        if (player_Prefeb == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + Application.loadedLevelName);
            GameObject temp_player=PhotonNetwork.Instantiate("Chracter/" + player_Prefeb.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            temp_player.GetComponent<Player>().chracter_name = LobbyPlayerlist.Instance.Charcter_list[LobbyPlayerlist.Instance.current_chracter_number].name;
            Debug.Log("캐릭터 이름 :"+ temp_player.GetComponent<Player>().chracter_name);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {            
            stream.SendNext(current_order);
        }
        else
        {            
            this.current_order = (Game_order_Name)stream.ReceiveNext();
        }
    }

    public void LateUpdate()
    {
        //자기자신이 아닐 때, 하지않는다.
        if (!photonView.isMine)
            return;

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

}
