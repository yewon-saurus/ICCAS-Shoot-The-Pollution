using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletCtrl : MonoBehaviour
{

    // 속도
    float speed = 100f;

    // 총알 삭제 시간
    float delay = 0.5f;
    void Update()
    {
        // 총알 속도 설정
        float Move = speed * Time.deltaTime;

        // 총알의 이동 방향 설정
        transform.Translate(Vector3.forward * Move);

        // 총알 삭제 설정 
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            Destroy(gameObject);
        }
    }
}