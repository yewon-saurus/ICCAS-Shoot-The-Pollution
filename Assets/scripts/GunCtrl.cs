using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GunCtrl : MonoBehaviour
{
    // 총 발사음
    public AudioClip sndFire;
    // 장전음
    public AudioClip sndCocking;
    //배경음악
    public AudioClip sndStage;
    //게임 오버음
    public AudioClip sndOver;

    //프리팹 exp: 폭파 화염, gunFire: 총구화염, fireBullet: 총알
    public Transform exp, gunFire, fireBullet;
    // 쏘아야 할 것들
    public Transform co2;

    // 탄피, 총구위치
    Transform bulletCase, spPoint;

    // 실패 횟수
    static public int miss;

    // 성공 횟수
    int hit;

    // 남은 총알
    int bulletCnt;

    // 시작 시간, 종료 시간
    float startTime, overTime;
    bool gameOver = false;

    // 화면 폭, 높이
    int width, height;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Cursor.visible = false;

        SetStage();
    }
    private void Update()
    {
        // 5번 미스 되면 게임 오버
        if (miss >= 5)
        {
            gameOver = true;
            overTime = Time.time;
        }

        if (gameOver)
        {
            return;
        }

        // Esc 누르면 게임종료
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     EditorApplication.isPlaying = false;
        // }

        RotateGun();
        MakeCo2();

        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
        }
        if (bulletCnt < 10 && Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ChargeGun());
        }

    }

    // 총 회전 
    void RotateGun()
    {
        //카메라부터 거리를 설정
        Vector3 pos = Input.mousePosition;

        // 화면의 기준으로 총의 움직임을 제한한다
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);

        // 카메라로부터의 거리
        pos.z = 13.2f;
        // 마우스 위치를 월드 좌표로 변환
        Vector3 view = Camera.main.ScreenToWorldPoint(pos);

        //총의 회전
        transform.LookAt(view);

    }

    void FireGun()
    {
        // 실탄 장전 애니메이션이 진행중이면 발사금지
        if (GetComponent<Animation>().isPlaying)
        {
            return;
        }

        RaycastHit hit;

        // 카메라 시점에서 커서 위치 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity);

        // 총을 클릭하면 실탄 재장전
        if (hit.transform.tag == "Gun" && bulletCnt < 10)
        {
            StartCoroutine(ChargeGun());
            return;
        }

        // 총구 앞 화면
        Instantiate(gunFire, spPoint.position, Quaternion.identity);
        // 총탄 발사 
        Instantiate(fireBullet, spPoint.position, spPoint.rotation);
        // 탄피 애니메이션
        GetComponent<Animation>().Play("Fire");

        CheckTarget(hit);

        // 남은 실탄 수 처리
        bulletCnt--;
        if (bulletCnt <= 0)
        {
            StartCoroutine(ChargeGun());
        }
    }

    // 목표물 적중여부 판정
    void CheckTarget(RaycastHit hit)
    {
        switch (hit.transform.tag)
        {
            // LetsShoot 종류의 obj 명중시 점수 처리및 폭발이팩트 생성및 제거
            case "co2":
                this.hit++;
                Instantiate(exp, hit.transform.position, Quaternion.identity);
                Destroy(hit.transform.gameObject);
                break;

            // DontShoot 종류의 obj 명중시 미스 처리
            // case "Bird":
            //     Instantiate(gunFire, hit.transform.position, Quaternion.identity);
            //     miss++;
            //     // hit.transform.SendMessage("DeadBird", SendMessageOptions.DontRequireReceiver);
            //     break;

        }
    }

    // 장전 애니메이션 
    IEnumerator ChargeGun()
    {
        while (GetComponent<Animation>().isPlaying)
        {
            yield return 0;
        }

        GetComponent<Animation>().Play("ChargeBullet");
        yield return new WaitForSeconds(0.5f);
        bulletCnt = 10;
    }

    // co2 생성
    void MakeCo2()
    {
        if (Random.Range(0, 1000) > 970 && !GetComponent<Animation>().isPlaying)
        {
            if (Random.Range(0,100) < 70)
            {
                Instantiate(co2);
            }
            // else
            // {
            //     Instantiate(bird);
            // }
        }
    }

    // 장전 효과음
    void SoundClick()
    {
        bulletCase.gameObject.GetComponent<Renderer>().enabled = true;
        AudioSource.PlayClipAtPoint(sndCocking, Vector3.zero);
    }

    // 탄 발사 효과음
    void SoundFire()
    {
        bulletCase.gameObject.GetComponent<Renderer>().enabled = true;
        AudioSource.PlayClipAtPoint(sndFire, Vector3.zero);
    }

    // 탄피 감추기 애니메이션
    void HideBullet()
    {
        bulletCase.gameObject.GetComponent<Renderer>().enabled = false;
    }
    void SetStage()
    {
        width = Screen.width;
        height = Screen.height;

        spPoint = GameObject.Find("spPoint").transform;

        bulletCase = transform.Find("Cylinder");

        bulletCase.gameObject.GetComponent<Renderer>().enabled = false;

        startTime = Time.time;
        hit = miss = 0;
        bulletCnt = 10;
        gameOver = false;

        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    void OnGUI()
    {
        // GUI.skin = skin;

        // 게임 진행 시간 
        float time = Time.time - startTime;

        if (!gameOver)
        {
            // 커서 위치에 조준점 표시
            GUI.DrawTexture(new Rect(Input.mousePosition.x - 24, height - Input.mousePosition.y - 24, 48, 48),
                            Resources.Load("crossHair") as Texture2D);
        }
        else
        {
            time = overTime - startTime;
        }

        // 남은 실탄수 표시
        for (int i = 1; i <= bulletCnt; i++)
        {
            GUI.DrawTexture(new Rect(i * 12, height - 20, 8, 16), Resources.Load("bullet") as Texture2D);
        }

        // 점수 등 변수 표시
        string sHit = "<size='30'>HIT : " + hit + "</size>";
        string sMiss = "<size='30'>MISS : " + miss + "</size>";
        string sTime = "<color='yellow'><size='30'>Time : " + (int)time + "</size></color>";

        GUI.Label(new Rect(30, 20, 120, 40), sHit);
        GUI.Label(new Rect(width / 2 - 40, 20, 160, 40), sTime);
        GUI.Label(new Rect(width - 120, 20, 120, 40), sMiss);

        // string msg = "Shoot : Left Btn  Charge : Gun Click";
        // GUI.Label(new Rect(width - 380, height - 40, 380, 40), msg);

        // 게임오버 처리
        if (gameOver)
        {
            Cursor.visible = true;
            if (GetComponent<AudioSource>().clip != sndOver)
            {
                GetComponent<AudioSource>().clip = sndOver;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
            }

            // Play Game 버튼 생성 및 버튼On 시 게임다시 진행
            if (GUI.Button(new Rect(width / 2 - 70, height / 2 - 50, 140, 60), "Play Game"))
            {
                Application.LoadLevel("MainGame");
            }

            // 게임에서 나옴
            // if (GUI.Button(new Rect(width / 2 - 70, height / 2 + 50, 140, 60), "Quit Game"))
            // {
            //     EditorApplication.isPlaying = false;
            // }
        }
    }
}