using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PhotonGameManager : Photon.PunBehaviour, IPunObservable
{
    #region Public 변수
    //싱글톤
    private static PhotonGameManager instance;
    public static PhotonGameManager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<PhotonGameManager>();
            if (!instance)
                Debug.Log("PhotonGameManager instance not find");
            return instance;
        }

        set { instance = value; }
    }


    #endregion
    #region public 변수
    //플레이어 접속 인원 아이디체크
    public Text[] text_player_list;
    public Text[] text_player_chracter_list;
    //플레이어 리스트클래스
    public LobbyPlayerlist player_list_class;

    //로비 패널과 텝페널
    public GameObject tapPannel;
    public GameObject lobbyPannel;
    #endregion

    private bool inGameorRobby = false;
    //포톤뷰
    private static PhotonView ScenePhotonView;


    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        player_list_class = LobbyPlayerlist.Instance;
        // 포톤네트워크가 접속이 되지 않으면 메인 화면으로 돌아간다.
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("PunBasics-Launcher");

            return;
        }

        //마스터클라이언트라면 목록 추가와 갱신을 start에서 해준다.
        if (PhotonNetwork.isMasterClient)
        {
            player_list_class.current_player_number = 0;
            player_list_class.playerList[0] = PhotonNetwork.player.NickName;
            Update_player_list();
        }
        ScenePhotonView = this.GetComponent<PhotonView>();
    }

    void LateUpdate()
    {
        //게임 안에 들어가 있다면?
        if(inGameorRobby)
        {
            //텝키를 누르면 템패널이 뜨게 한다.
            if(Input.GetKeyDown(KeyCode.Tab)){
                tapPannel.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                tapPannel.SetActive(false);
            }
        }
    }

    #region 게임 시작 버튼을 누르면 세팅되는 값들
   

    //마스터클라이언트만 누르면 반응하는 게임 시작 버튼
    public void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("마스터클라이언트가 시작을 눌렀습니다.");
            PhotonNetwork.LoadLevel("GameScene");
            ScenePhotonView.RPC("StartGameSetting", PhotonTargets.All);
        }
    }


    #endregion


    #region 방떠나기 
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region 플레이어 접속 및 디스커넥트
    //플레이어 접속확인 함수
    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting      
            for (int i = 0; i < 6; ++i) {
                if (player_list_class.playerList[i] == "")
                {
                    player_list_class.playerList[i] = other.NickName;
                    break;
                }
            }      
            SendPlayer(player_list_class.playerList);                  
        }
    }
    //플레이어 디스커넥팅 함수
    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects 
            for (int i = 0; i < 6; ++i)
            {
                if (player_list_class.playerList[i] == other.NickName)
                {
                    player_list_class.playerList[i] = "";
                    break;
                }
            }
            SendPlayer(player_list_class.playerList);
        }
    }
    #endregion

#region RPC 함수들
    public static void SendPlayer(string[] list)
    {
        Debug.Log("rpc:player ");
        ScenePhotonView.RPC("Send_Playerlist", PhotonTargets.All, list);
    }

    public static void ChangePlayer_Chracter(int num,string playerID)
    {
        Debug.Log("rpc:Chracter ");
        ScenePhotonView.RPC("Change_Chracter", PhotonTargets.All, num,playerID);
    }
    [PunRPC]
    void Send_Playerlist(string[] list)
    {
        player_list_class.playerList = list;
        player_list_class.SetPlayer_number();
        Update_player_list();
        Debug.Log("플레이어리스트갱신");
    }

    [PunRPC]
    void Change_Chracter(int num, string name)
    {
        text_player_chracter_list[num].text = name;
        Debug.Log(num + "번호 변경 : " + name);
    }
    [PunRPC]
    public void StartGameSetting()
    {
        inGameorRobby = true;
        tapPannel.SetActive(false);
        lobbyPannel.SetActive(false);
        Debug.Log("게임 시작 세팅(패널숨기기)");
    }

    #endregion



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {       
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            //stream.SendNext(player_list_class.current_player_number);
        }
        else
        {
            // Network player, receive data
            //this.player_list_class.current_player_number = (int)stream.ReceiveNext();
        }        
    }    

    #region private 함수
    private void Update_player_list()
    {        
       for(int i=0;i< 6; ++i)
        {
            text_player_list[i].text = player_list_class.playerList[i];
        }
    }

   
    #endregion
}
