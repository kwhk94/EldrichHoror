using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_move_scrip : MonoBehaviour {
    public RectTransform UI;
    public RectTransform MoveOffset_Object_target;
    public RectTransform MoveOffset_Object_standard;
    public float time;


    public void UI_lerp_target()
    {
        //ui가 목적지에 lerp시킨다.
        UI.localPosition = Vector3.Lerp(UI.localPosition, MoveOffset_Object_target.localPosition, Time.deltaTime);
        
    }

    public void UI_lerp_standard()
    {
        //ui가 목적지에 lerp시킨다.
        UI.localPosition = Vector3.Lerp(UI.localPosition, MoveOffset_Object_standard.localPosition, Time.deltaTime);

        if (Vector3.Distance(UI.localPosition, MoveOffset_Object_standard.localPosition)<0.1f)
        {
            UI.localPosition = MoveOffset_Object_standard.localPosition;
            GameSystem.Instance.UI_eventlist -= UI_lerp_standard;
        }
    }

    public void Onpointerenter()
    {
        GameSystem.Instance.UI_eventlist += UI_lerp_target;
        GameSystem.Instance.UI_eventlist -= UI_lerp_standard;
    }

    public void Onpointerout()
    {
        GameSystem.Instance.UI_eventlist -= UI_lerp_target;
        GameSystem.Instance.UI_eventlist += UI_lerp_standard;       
    }


}
