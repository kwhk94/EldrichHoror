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

	//행동, 조우,신화 단계
	public enum Game_order_Name {Action =0x1, Meeting = 0x2, Myth = 0x4 };
	//행동 단계에서의 행동들, 이동, 휴식, 자산획득, 이동준비,거래 , 구성물 행동
	public enum Action_Name { Move = 0x1, Rest = 0x2, Shop = 0x4 ,Ticket = 0x8, Deal = 0x10, Skill = 0x20 };

	//현재 단계
	public Game_order_Name current_order = Game_order_Name.Myth;



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
