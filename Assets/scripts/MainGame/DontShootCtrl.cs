using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontShootCtrl : MonoBehaviour
{
    // 명중시 프리팹 연결 오브젝트
    public Transform oCtrl;
    // 명중?
    bool isDead = false;

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
            // GunCtrl.miss++;
            Destroy(gameObject);
        }

        // 난이도에 따라 속력 조절
        if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 1) {
            speed = Random.Range(3, 4f);
            // Debug.Log("속도 조절: 난이도 1");
        }
        else if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 2) {
            speed = Random.Range(3, 5f);
            // Debug.Log("속도 조절: 난이도 2");
        }
        else if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 3) {
            speed = Random.Range(3, 5f);
            // Debug.Log("속도 조절: 난이도 3");
        }
        else if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 4) {
            speed = Random.Range(3, 6f);
            // Debug.Log("속도 조절: 난이도 4");
        }
        else if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 5) {
            speed = Random.Range(4, 6f);
            // Debug.Log("속도 조절: 난이도 5");
        }
        else if (GameObject.Find("Gun").GetComponent<GunCtrl>().difficulty == 6) {
            speed = Random.Range(5, 7f);
            // Debug.Log("속도 조절: 난이도 6");
        }
        else {
            speed = Random.Range(4, 7f);
            // Debug.Log("속도 조절: 난이도 7");
        }
    }

    // Ouch! 쏘면 안되는 것을 쐈을 때
    void Ouch()
    {
        isDead = true;

        // 명중 됐을 당시 오브젝트 위치 계산하여 Ouch! 오브젝트 위치 설정
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.2f, 8);
        Quaternion rot = Quaternion.identity;

        rot.eulerAngles = new Vector3(0, 0, 0);
        Instantiate(oCtrl, pos, rot);
    }

    void SetPosition()
    {
        speed = Random.Range(2, 3f);
        Debug.Log("속도 조절: 난이도 1");

        // 추락 속도 설정
        gravity = 2f;

        // 이동 방향 설정
        dirX = dir[Random.Range(0, 2)];

        // 화면의 좌우 위치
        float posX = -8 * dirX;

        // 높이 및 회전설정
        float posY = Random.Range(1.5f, 3);
        transform.position = new Vector3(posX, posY, 9);
        transform.eulerAngles = new Vector3(-50, 0, Random.Range(10, 20f) * dirX);
    }
}