using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetsShootCtrl : MonoBehaviour
{
    float speed; // 날아가는 속도
    float gravity; // 충력

    int[] dir = { -1, 1 };
    int dirX; // 이동방향
    void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // 이동 속도를 감속한다
        gravity -= 1.3f * Time.deltaTime;
        // 이동 방향과 속도를 설정

        // 회전을 줘서 월드좌표로 이동
        Vector3 move = new Vector3(speed * dirX, gravity, 0) * Time.deltaTime;
        transform.Translate(move, Space.World);


        // 화면에서 벗어나면 제거
        if (Mathf.Abs(transform.position.x) > 8 || transform.position.y < -3)
        {
            GunCtrl.miss++;
            Destroy(gameObject);
        }
        
    }

    void SetPosition()
    {
        // 랜덤으로 속도 설정
        speed = Random.Range(2, 5f);

        // 추락 속도 설정
        gravity = 2f;

        // 이동 방향 설정
        dirX = dir[Random.Range(0, 2)];

        // 화면의 좌우 위치
        float posX = -8 * dirX;

        // 높이 및 회전설정
        float posY = Random.Range(2.5f, 4);
        transform.position = new Vector3(posX, posY, 9);
        transform.eulerAngles = new Vector3(-50, 0, Random.Range(10, 20f) * dirX);
    }
}