using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SynopCtrl : MonoBehaviour
{
    public Button btnGameStart;
    public Button btnBack;

    void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainGame");
        });

        btnBack.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("FirstTitle");
        });
    }
}
