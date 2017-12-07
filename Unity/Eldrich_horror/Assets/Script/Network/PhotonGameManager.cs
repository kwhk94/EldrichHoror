using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PhotonGameManager : Photon.PunBehaviour,IPunObservable
{
    #region Public 변수
    //싱글톤
    static public PhotonGameManager Instance;
    #endregion
    //플레이어 접속 인원 아이디체크
    public Text[] text_player_list;
    public Text[] text_player_chracter_list;

    //플레이어 이름 리스트 (이름 갱신을 위한 목록
    public string[] m_player_list = new string[6];

    public LobbyPlayerlist player_list_class;

    

    //포톤뷰
    private static PhotonView ScenePhotonView;    
    // Use this for initialization
    void Start () {
        Instance = this;
        // 포톤네트워크가 접속이 되지 않으면 메인 화면으로 돌아간다.
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("PunBasics-Launcher");

            return;
        }

        if(PhotonNetwork.isMasterClient)
        {
            string name = PhotonNetwork.masterClient.NickName;
            m_player_list[0] = name;
            player_list_class.current_player_number = 0;
            Update_player_list();
        }
        ScenePhotonView = this.GetComponent<PhotonView>();
    }


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


    //플레이어 접속확인 함수
    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected() " + other.NickName); // not seen if you're the player connecting
            for(int i=0; i<6;++i)
            {
                if (m_player_list[i] == "")
                {
                    m_player_list[i] = other.NickName;
                    
                    break;
                }
            }
            Update_player_list();          
            SendPlayer(m_player_list);                  
        }
    }   


    public static void SendPlayer(string[] playerID)
    {
        Debug.Log("rpc:player ");
        ScenePhotonView.RPC("Send_Playerlist", PhotonTargets.All, playerID);
    }

    public static void ChangePlayer_Chracter(int num,string playerID)
    {
        Debug.Log("rpc:Chracter ");
        ScenePhotonView.RPC("Change_Chracter", PhotonTargets.All, num,playerID);
    }



    //플레이어 디스커넥팅 함수
    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisconnected() " + other.NickName); // seen when other disconnects
            for (int i = 0; i < 6; ++i)
            {
                if (m_player_list[i] == other.NickName)
                {
                    m_player_list[i] = "";
                    break;
                }
            }
            Update_player_list();
        }
    }

  
    //마스터클라이언트만 누르면 반응하는 게임 시작 버튼
    public void StartGame()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("마스터클라이언트가 시작을 눌렀습니다.");
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

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
       for(int i=0;i<6;++i)
        {
            text_player_list[i].text = m_player_list[i];
        }
    }


    [PunRPC]
    void Send_Playerlist(string[] list)
    {
        m_player_list = list;
        Debug.Log("rpc_playerlist");
        Update_player_list();
        Debug.Log("플레이어리스트갱신");
    }

    [PunRPC]
    void Change_Chracter(int num,string name)
    {
        text_player_chracter_list[num].text = name;
    }
    #endregion
}
