using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour {
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

    public UI_moveicon ruin_icon;
    public UI_moveicon symptom_icon;

    public delegate void UI_event();
    public event UI_event UI_eventlist;

    public void LateUpdate()
    {
        //멸망 토큰의 이동 애니메이션
        if(ruin_icon.currentnum!=ruin_icon.targetnum)
        {
            ruin_icon.move_icon();
        }
        //징조 토큰 이동 애니메이션
        if(symptom_icon.currentnum != symptom_icon.targetnum)
        {
            symptom_icon.move_icon();
        }

        if(UI_eventlist != null)
            UI_eventlist();
    }

}
