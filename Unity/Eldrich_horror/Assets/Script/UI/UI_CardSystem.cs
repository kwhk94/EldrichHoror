using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CardSystem : MonoBehaviour {
    #region 싱글톤
    private static UI_CardSystem instance;
    public static UI_CardSystem Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<UI_CardSystem>();
            if (!instance)
                Debug.Log("UI_CardSystem instance not find");
            return instance;
        }

        set { instance = value; }
    }
    #endregion

    public GameObject actionCardlist;
    //카드 선택 UI offon
    public bool cardSelectonoff;
    private PhotonView photonview;
    private void Start()
    {
        photonview = GetComponent<PhotonView>();
    }


    public void SetActionCardlist(bool onoff)
    {
        actionCardlist.SetActive(onoff);
    }

    //카드 선택시 타입을 바꾼다.카드 UI를 숨긴다.
    public void MoveCardSelect()
    {
        //카드 셀렉트 불값을 onoff, 카드 UI를 끄기, 액션 타입 변경
        Player.instance.inout_bool = true;
        cardSelectonoff = true;
        SetActionCardlist(false);
        GameSystem.Instance.gameRule.actionName = Action_Name.Move;
        photonview.photonView.RPC("ChangeActionName", PhotonTargets.MasterClient, Action_Name.Move);
    }


    #region RPC 함수들

    [PunRPC]
    public void ChangeActionName(Action_Name type)
    {
        GameSystem.Instance.gameRule.actionName = type;
        Debug.Log("게임시스템 액션 이름 바꾸기 : " + type.ToString());
    }

    #endregion

}
