using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard_size : MonoBehaviour {

    public Vector3 move_vec3;

    public CameraSystem.outvec send_outvec;

    private void LateUpdate()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if ((send_outvec & CameraSystem.outvec.bottom)!=0 | (send_outvec & CameraSystem.outvec.top) != 0)          //보내는타입이 위와 아래라면
        {
            if (targetScreenPos.y < Screen.height && targetScreenPos.y > 0)                                        //y축만 보면 된다.
            {                
                CameraSystem.Instance.outveclist |= send_outvec;
            }
            else
            {                
                CameraSystem.Instance.outveclist &= ~send_outvec;
            }
        }
        if ((send_outvec & CameraSystem.outvec.right) != 0 | (send_outvec & CameraSystem.outvec.left) != 0)             //보내는타입이 왼쪽,오른쪽이라면
        {
            if (targetScreenPos.x < Screen.width && targetScreenPos.x > 0)                                                //x축만 보면 된다.
            {                
                CameraSystem.Instance.outveclist |= send_outvec;
            }
            else
            {               
                CameraSystem.Instance.outveclist &= ~send_outvec;
            }
        }

    }



}
