using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }
    private void Update()
    {
        RotateGun();
    }

    //총회전 
    void RotateGun()
    {
        //카메라부터 거리를 설정
        Vector3 pos = Input.mousePosition;

        //화면의 기준으로 총의 움직임을 제한한다
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);

        //카메라로 부터의 거리
        pos.z = 13.2f;
        //마우스 위치를 월드 좌표로 변환
        Vector3 view = Camera.main.ScreenToWorldPoint(pos);

        //총의 회전
        transform.LookAt(view);

    }

}