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
    // 배경 음악
    public AudioClip sndStage;
    // 게임 오버음
    public AudioClip sndOver;

    //프리팹 exp: 폭파 화염, gunFire: 총구화염, fireBullet: 총알
    public Transform exp, gunFire, fireBullet;
    // 쏘아야 할 것들
    public Transform lets1, lets2, lets3, lets4, lets5;
    // 쏘면 안되는 것들
    public Transform dont1, dont2, dont3, dont4;

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

    // 게임 진행 시간 
    float time;

    // 난이도 조절을 위한 변수
    public int difficulty;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Cursor.visible = false;

        SetStage();
    }
    private void Update()
    {
        // 10번 미스 되면 게임 오버
        if (miss >= 10)
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

        // 게임 진행 시간 
        time = Time.time - startTime;

        if (time <= 30) {
            this.difficulty = 1;
            // Debug.Log("difficulty: " + this.difficulty);
        }
        else if (time > 30 && time <= 60) {
            this.difficulty = 2;
            Debug.Log("difficulty: " + this.difficulty);
        }
        else if (time > 60 && time <= 90) {
            this.difficulty = 3;
        }
        else if (time > 90 && time <= 120) {
            this.difficulty = 4;
        }
        else if (time > 120 && time <= 180) {
            this.difficulty = 5;
        }
        else if (time > 180 && time <= 240) {
            this.difficulty = 6;
        }
        else {
            this.difficulty = 7;
        }

        RotateGun();
        MakeForShoot();

        if (Input.GetMouseButtonDown(0))
        {
            FireGun();
        }
        if (bulletCnt < 10 && Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ChargeGun());
        }

    }

    public int getDifficulty() {
        return this.difficulty;
    }

    // 총 회전 
    void RotateGun()
    {
        // 카메라부터 거리를 설정
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
        // 남은 실탄 수 처리
        bulletCnt--;
        if (bulletCnt == 0)
        {
            StartCoroutine(ChargeGun());
        }

        // 실탄 장전 애니메이션이 진행중이면 발사금지
        if (GetComponent<Animation>().isPlaying)
        {
            return;
        }

        RaycastHit hit;

        // 카메라 시점에서 커서 위치 계산
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, Mathf.Infinity);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("충돌함");
            Debug.Log("충돌체 이름: " + hit.collider.gameObject.name);
        }

        // 총구 앞 화면
        Instantiate(gunFire, spPoint.position, Quaternion.identity);
        // 총탄 발사 
        Instantiate(fireBullet, spPoint.position, spPoint.rotation);
        // 탄피 애니메이션
        GetComponent<Animation>().Play("Fire");

        CheckTarget(hit);
    }

    // 목표물 적중여부 판정
    void CheckTarget(RaycastHit hit)
    {
        switch (hit.transform.tag)
        {
            // LetsShoot 종류의 obj 명중시 점수 처리및 폭발이팩트 생성및 제거
            case "LetsShoot":
                this.hit++;
                Instantiate(gunFire, hit.transform.position, Quaternion.identity);
                Destroy(hit.transform.gameObject);
                break;

            case "DontShoot":
                miss++;
                Instantiate(gunFire, hit.transform.position, Quaternion.identity);
                hit.transform.SendMessage("Ouch", SendMessageOptions.DontRequireReceiver);
                Destroy(hit.transform.gameObject);
                break;

            // DontShoot 종류의 obj 명중시 미스 처리

            // case "Clay":
            //     this.hit++;
            //     Instantiate(exp, hit.transform.position, Quaternion.identity);
            //     Destroy(hit.transform.gameObject);
            //     break;

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

    // 개체 생성
    void MakeForShoot()
    {
        int howMuch = 995;
        
        if (this.difficulty == 1) {
            // time 0 ~ 30
            // LetsShoot 1개
            // speed 1단계
            // 나타나는 갯수도 적음
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    Instantiate(lets1);
                }
            }
        }
        else if (this.difficulty == 2) {
            // time 30 ~ 60
            // LetsShoot 1개, DontShoot 1개
            // speed 2단계
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    Instantiate(lets1);
                }
                else
                {
                    Instantiate(dont1);
                }
            }
        }
        else if (this.difficulty == 3 || this.difficulty == 4) {
            // time 60 ~ 90, 90 ~ 120
            // LetsShoot 2개, DontShoot 2개
            // speed 3단계, 4단계
            howMuch = 990;
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    if (Random.Range(0,100) < 50)
                    {
                        Instantiate(lets1);
                    }
                    else
                    {
                        Instantiate(lets2);
                    }
                    
                }
                else
                {
                    if (Random.Range(0,100) > 80)
                    {
                        Instantiate(dont1);
                    }
                    else
                    {
                        Instantiate(dont2);
                    }
                }
            }
        }
        else if (this.difficulty == 5) {
            // time 120 ~ 180
            // LetsShoot 3개, DontShoot 3개
            // speed 5단계
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    if (Random.Range(0,100) < 30)
                    {
                        Instantiate(lets1);
                    }
                    else if (Random.Range(0,100) >= 30 && Random.Range(0,100) < 60)
                    {
                        Instantiate(lets2);
                    }
                    else
                    {
                        Instantiate(lets3);
                    }
                }
                else
                {
                    if (Random.Range(0,100) > 90)
                    {
                        Instantiate(dont1);
                    }
                    else if (Random.Range(0,100) <= 90 && Random.Range(0,100) > 80)
                    {
                        Instantiate(dont2);
                    }
                    else
                    {
                        Instantiate(dont3);
                    }
                }
            }
        }
        else if (this.difficulty == 6) {
            // time 180 ~ 240
            // LetsShoot 4개, DontShoot 4개
            // speed 6단계
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    if (Random.Range(0,100) < 20)
                    {
                        Instantiate(lets1);
                    }
                    else if (Random.Range(0,100) >= 20 && Random.Range(0,100) < 40)
                    {
                        Instantiate(lets2);
                    }
                    else if (Random.Range(0,100) >= 40 && Random.Range(0,100) < 50)
                    {
                        Instantiate(lets3);
                    }
                    else
                    {
                        Instantiate(lets4);
                    }
                }
                else
                {
                    if (Random.Range(0,100) > 90)
                    {
                        Instantiate(dont1);
                    }
                    else if (Random.Range(0,100) <= 90 && Random.Range(0,100) > 85)
                    {
                        Instantiate(dont2);
                    }
                    else if (Random.Range(0,100) <= 85 && Random.Range(0,100) > 80)
                    {
                        Instantiate(dont3);
                    }
                    else
                    {
                        Instantiate(dont4);
                    }
                }
            }
        }
        else {
            // time 240 ~
            // LetsShoot 5개, DontShoot 4개
            // speed 7단계
            howMuch = 980;
            if (Random.Range(0, 1000) > howMuch && !GetComponent<Animation>().isPlaying)
            {
                if (Random.Range(0,100) < 70)
                {
                    if (Random.Range(0,100) < 40)
                    {
                        Instantiate(lets1);
                    }
                    else if (Random.Range(0,100) >= 40 && Random.Range(0,100) < 50)
                    {
                        Instantiate(lets2);
                    }
                    else if (Random.Range(0,100) >= 50 && Random.Range(0,100) < 60)
                    {
                        Instantiate(lets3);
                    }
                    else if (Random.Range(0,100) >= 60 && Random.Range(0,100) < 65)
                    {
                        Instantiate(lets4);
                    }
                    else
                    {
                        Instantiate(lets5);
                    }
                }
                else
                {
                    if (Random.Range(0,100) > 95)
                    {
                        Instantiate(dont1);
                    }
                    else if (Random.Range(0,100) <= 95 && Random.Range(0,100) > 90)
                    {
                        Instantiate(dont2);
                    }
                    else if (Random.Range(0,100) <= 90 && Random.Range(0,100) > 85)
                    {
                        Instantiate(dont3);
                    }
                    else
                    {
                        Instantiate(dont4);
                    }
                }
            }
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
        difficulty = 1;

        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }

    void OnGUI()
    {
        // GUI.skin = skin;

        if (!gameOver)
        {
            // 커서 위치에 조준점 표시
            GUI.DrawTexture(new Rect(Input.mousePosition.x - 24, height - Input.mousePosition.y - 24, 150, 150),
                            Resources.Load("crossHair") as Texture2D);
        }
        else
        {
            time = overTime - startTime;
        }

        // 남은 실탄수 표시
        for (int i = 1; i <= bulletCnt; i++)
        {
            GUI.DrawTexture(new Rect(i * 48, height - 80, 32, 64), Resources.Load("bullet") as Texture2D);
        }

        // 점수 등 변수 표시
        string sHit = "<size='60'>HIT : " + hit + "</size>";
        string sMiss = "<size='60'>MISS : " + miss + "</size>";
        string sTime = "<color='yellow'><size='60'>Time : " + (int)time + "</size></color>";

        GUI.Label(new Rect(60, 40, 240, 80), sHit);
        GUI.Label(new Rect(width / 2 - 80, 40, 320, 80), sTime);
        GUI.Label(new Rect(width - 240, 40, 240, 80), sMiss);

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
            if (GUI.Button(new Rect(width / 2 - 250, height / 2 - 100, 500, 200), "Play Game"))
            {
                Application.LoadLevel("MainGame");
            }
        }
    }
}