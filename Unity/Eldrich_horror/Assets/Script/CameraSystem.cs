using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
    private static CameraSystem instance;
    public static CameraSystem Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<CameraSystem>();
            if (!instance)
                Debug.Log("CameraSystem instance not find");
            return instance;
        }

        set { instance = value; }
    }

    public float camera_speed = 10f;
    public float camera_firstspeed = 10f;
    public float camera_max_speed = 100f;
    public float camera_acceleration = 10f;

    //줌인 줌 아웃 최대 크기 및 최소 크기, 현재 크기
    public float wheelmaxSize = 100f;
    public float wheelminSize = 0f;
    public float wheelcurrentSize = 50f;
    public float wheelspeed = 50f;

    private Vector3 moveDirection;
    private Vector3 forwardvec3;
    private Transform camera_pos;

    //카메라가 보드 밖으로 나간 방향
    public enum outvec {top = 0x1, bottom = 0x2, right = 0x4, left = 0x8};
    //카메라가 보드밖으로 나간 방향을 16진수로 표현
    public outvec outveclist;
    void Start() {
        camera_pos = GetComponent<Transform>();
        forwardvec3 = camera_pos.forward;
    }


    void LateUpdate() {
        //누르고 있는 키에 대한 방향 벡터 
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //보드 밖으로 나간 상태라면?
        if ( ((outveclist & outvec.bottom) == outvec.bottom && Input.GetAxis("Vertical")<0) ||                     //보드아래로
            ((outveclist & outvec.top) == outvec.top && Input.GetAxis("Vertical") > 0) )                          //보드 위로
        {
            moveDirection.z = 0.0f;
        }
        if ( ((outveclist & outvec.left) == outvec.left && Input.GetAxis("Horizontal") < 0) ||                     //보드왼쪽로
              ((outveclist & outvec.right) == outvec.right && Input.GetAxis("Horizontal") > 0) )                          //보드 오른쪽으로
        {
            moveDirection.x = 0.0f;
        }


        //키를 누르고 있는 중이라면?
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") !=0)
        {
            //카메라의 최대 속도가 아니라면 계속 가속도를 넣어준다.
            if(camera_speed<camera_max_speed)
                camera_speed += camera_acceleration;
            else{
                camera_speed = camera_max_speed;
            }
        }
        else
        {
            //누르고 있지 않다면 카메라의 초기 속력으로 유지한다.
            camera_speed = camera_firstspeed;
        }
        //카메라를 속도에 따라 이동시킨다.
        camera_pos.position += moveDirection * camera_speed * Time.deltaTime;

        //마우스 휠로 줌 아웃을 한다.
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && wheelcurrentSize<wheelmaxSize)
        {
            camera_pos.position -= forwardvec3* wheelspeed * Time.deltaTime;
            wheelcurrentSize += Time.deltaTime* wheelspeed;
        }
        //마우스 휠로 줌 인한다.
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && wheelcurrentSize > wheelminSize)
        {
            camera_pos.position += forwardvec3* wheelspeed * Time.deltaTime;
            wheelcurrentSize -= Time.deltaTime* wheelspeed;
        }
    }
    
    
}
