using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCtrl : MonoBehaviour
{
    public Text highScoreText;
    public Text scoreText;

    public GameObject newRecord;
    public GameObject gameOver;

    void Awake()
    {
        newRecord.SetActive(false);
        gameOver.SetActive(false);

        string tempSaved = GameObject.Find("Gun").GetComponent<GunCtrl>().savedScore.ToString("0");
        string tempHit = GameObject.Find("Gun").GetComponent<GunCtrl>().hit.ToString("0");
        bool tempHigh = GameObject.Find("Gun").GetComponent<GunCtrl>().isHigh;

        highScoreText.text = tempSaved;
        scoreText.text = tempHit;

        // Debug.Log(GameObject.Find("Gun").GetComponent<GunCtrl>().savedScore);
        
        if (tempHigh == true) {
            // high score 달성 시
            newRecord.SetActive(true);
            Debug.Log("신기록 달성!(ResultCtrl.cs)");
        }
        else {
            gameOver.SetActive(true);
            Debug.Log("신기록 x");
        }
    }

    void Start()
    {
        GameObject.Find("Gun").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
