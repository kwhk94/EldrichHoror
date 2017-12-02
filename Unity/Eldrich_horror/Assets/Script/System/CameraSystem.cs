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
    public float wheelmaxSize = 130f;
    public float wheelminSize = 40f;
    public float wheelspeed = 50f;

    //회전 속도
    public float xSensitivity = 1.0f;
    public float ySensitivity = 1.0f;

    private Vector3 moveDirection;
    private Vector3 forwardvec3;
    private Rigidbody camera_rig;

    private float xRot = 0.0f, yRot = 0.0f;
    //수평, 수직 회전 각
    private float verticalRotation = 0f, horizontalRotation = 0f;
    private bool right_button_onoff = false;


    void Start() {
        camera_rig = GetComponent<Rigidbody>();
        forwardvec3 = transform.forward;
        horizontalRotation = transform.eulerAngles.y;
        verticalRotation = transform.eulerAngles.x;
    }


    void LateUpdate() {
        InputUpdate();       

        
        if (right_button_onoff == true){
            xRot = Input.GetAxis("Mouse X") * xSensitivity;
            yRot = Input.GetAxis("Mouse Y") * ySensitivity;
            horizontalRotation += xRot;
            verticalRotation -= yRot;
            transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);            //쿼터니언 카메라 회전
        }
        else {
            //카메라를 속도에 따라 이동시킨다.
            camera_rig.MovePosition(transform.position + moveDirection * camera_speed * Time.deltaTime);

        }



        //마우스 휠로 줌 아웃을 한다.
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < wheelmaxSize)
        {
            transform.position = (transform.position -forwardvec3* wheelspeed * Time.deltaTime);            
        }
        //마우스 휠로 줌 인한다.
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > wheelminSize)
        {
            transform.position = (transform.position +forwardvec3* wheelspeed * Time.deltaTime);
        }
    }

    private void InputUpdate()
    {
        //누르고 있는 키에 대한 방향 벡터 
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //키를 누르고 있는 중이라면?
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //카메라의 최대 속도가 아니라면 계속 가속도를 넣어준다.
            if (camera_speed < camera_max_speed){
                camera_speed += camera_acceleration;
            }
            else{
                camera_speed = camera_max_speed;
            }
        }
        else{
            //누르고 있지 않다면 카메라의 초기 속력으로 유지한다.
            camera_speed = camera_firstspeed;
        }

        if (Input.GetMouseButtonDown(1))
        {
            right_button_onoff = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            right_button_onoff = false;
        }


    }
}
