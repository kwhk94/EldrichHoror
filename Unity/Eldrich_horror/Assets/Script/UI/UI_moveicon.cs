using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_moveicon : MonoBehaviour {
    public RectTransform[] move_list;
    
    private float time;
    public float speed = 1.0f/3.0f;
    public int currentnum;
    public int targetnum;

    private RectTransform m_recttransform;
    private void Start()
    {
        currentnum = targetnum;
        m_recttransform = GetComponent<RectTransform>();
        m_recttransform.position = move_list[currentnum].position;        
    }

    public void move_icon()
    {
        m_recttransform.position =Vector3.Lerp(move_list[currentnum].position, move_list[targetnum].position, time);
        time += Time.deltaTime*speed;   //일정 속도에 따라 이동 이때 속도는 원하는 시간의 역수이다.
        if (time > 1){                  //목적지에 도착하게 되면 시간을 0으로
            time = 0f;                  //현재위치번호를 목표위치번호로 바꾼다.
            currentnum = targetnum;
        }        
    }
    public void ChangeTargetnum(int num)
    {
        if(currentnum + 1 >= move_list.Length){ //리스트 길이보다 크면 0으로 돌아간다.
            targetnum = 0;
        }
        else
            targetnum = num;
    }

}
