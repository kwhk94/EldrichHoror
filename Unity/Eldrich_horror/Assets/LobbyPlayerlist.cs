using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LobbyPlayerlist : Photon.PunBehaviour
{
    #region 싱글톤
    private static LobbyPlayerlist instance;
    public static LobbyPlayerlist Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<LobbyPlayerlist>();
            if (!instance)
                Debug.Log("LobbyPlayerlist instance not find");
            return instance;
        }

        set { instance = value; }
    }
#endregion

    public GameObject[] Charcter_list;

    public int current_player_number = 0;
    public int current_chracter_number = 0;
    public string[] playerList = new string[6];
    public void Start()
    {
        instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

#region 플레이어 번호 확인 및 갱신
    public void SetPlayer_number()
    {
        for (int i = 0; i < 6; ++i)
        {            
            if (playerList[i] == PhotonNetwork.player.NickName)
            {
                current_player_number = i;
                break;
            }
        }
        Debug.Log(current_player_number + "플레이어 넘버");
    }
    #endregion

#region 버튼 클릭시, 플레이어의 캐릭터 갱신 및 바꾸기
    public void Change_playernumber()
    {
        //플레이어 캐릭터 변경
        current_chracter_number += 1;
        if (current_chracter_number > Charcter_list.Length-1)
            current_chracter_number = 0;
        //플레이어 캐릭터 번호와 종류를 보냄(추후 중복방지를 위함
        PhotonGameManager.ChangePlayer_Chracter(current_player_number, Charcter_list[current_chracter_number].name);
    }
#endregion
}
