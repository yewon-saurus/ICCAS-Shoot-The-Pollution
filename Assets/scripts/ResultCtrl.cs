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
        highScoreText.text = GameObject.Find("Gun").GetComponent<GunCtrl>().savedScore.ToString("0");
        scoreText.text = GameObject.Find("Gun").GetComponent<GunCtrl>().hit.ToString("0");

        // Debug.Log(GameObject.Find("Gun").GetComponent<GunCtrl>().savedScore);
        
        if (GameObject.Find("Gun").GetComponent<GunCtrl>().isHigh == true) {
            // high score 달성 시
            newRecord.SetActive(true);
            Debug.Log("신기록 달성!(ResultCtrl.cs)");
        }
        else {
            gameOver.SetActive(true);
            Debug.Log("신기록 x");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
