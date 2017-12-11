using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type_loc { Main_green = 0x1, Main_orange = 0x2, Main_purple = 0x4, Main_other = 0x8 ,Main_sub = 0x80};
public enum Type_sub {sub_city = 0x10, sub_jungle = 0x20, sub_sea = 0x40 };



public class Location : MonoBehaviour {

    public Type_loc MainType;
    public Type_sub SubType;
    public bool moveSelect = false;
    //경로 리스트
    public MeshRenderer[] Location_Path;
    //장소 리스트
    public Location[] Location_list;

    private RaycastHit hit;
    private int LocationMask;

    public void Start()
    {
        LocationMask = LayerMask.GetMask("Location");
    }

    private void OnMouseEnter()
    {
        if (moveSelect)
        {
            LocationOnOff(true);
            return;
        }
        LocationPathOnOff(true);
    }
    private void OnMouseExit()
    {
        if (moveSelect)
        {
            LocationOnOff(false);
            return;
        }
        LocationPathOnOff(false);
    }

    private void OnMouseDown()
    {
        //플레이어 자기 자신의 턴에
        if (!Player.instance.photonView.isMine)
            return;
        //움직임이 가능하고, 이동단계일 때
        if(moveSelect && GameSystem.Instance.gameRule.gameOrder == Game_order_Name.Action && GameSystem.Instance.gameRule.actionName == Action_Name.Move)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LocationMask))
                Player.instance.agent.SetDestination(hit.point);
        }
    }

    public void LocationPathOnOff(bool onoff)
    {
        for (int i = 0; i < Location_Path.Length; ++i)
        {
            //마우스가 올려지면 경로를 보여준다.
            Location_Path[i].enabled = onoff;
        }
    }
    public void LocationOnOff(bool onoff)
    {
        Location_Path[0].enabled = onoff;
    }

    public void Location_listOnOff(bool onoff)
    {
        moveSelect = onoff;
        for (int i = 0; i < Location_list.Length; ++i)
        {
            Location_list[i].moveSelect = onoff;
        }
        for (int i = 0; i < Location_Path.Length; ++i)
        {
            //마우스가 올려지면 경로를 보여준다.
            Location_Path[i].enabled = onoff;
        }
    }



}
