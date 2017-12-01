using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Type_loc { Main_green = 0x1, Main_orange = 0x2, Main_purple = 0x4, Main_other = 0x8 ,Main_sub = 0x80};
public enum Type_sub {sub_city = 0x10, sub_jungle = 0x20, sub_sea = 0x40 };



public class Location : MonoBehaviour {
    public MeshRenderer [] Location_Path;    

    public Type_loc MainType;
    public Type_sub SubType;
    private void OnMouseEnter()
    {
        for(int i=0;i<Location_Path.Length ;++i)
        {
            //마우스가 올려지면 경로를 보여준다.
            Location_Path[i].enabled = true;
        }
    }
    private void OnMouseExit()
    {
        for (int i = 0; i < Location_Path.Length; ++i)
        {
            //마우스가 올려지면 경로를 꺼버린다.
            Location_Path[i].enabled = false;
        }
    }

   
}
