using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button btnGameStart;
    public Button btnGameClose;
    public Button btnGameSetting;

    void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            Debug.Log("Maaaaaaa");
            SceneManager.LoadScene("Synopsys Scene");

        });

        btnGameSetting.onClick.AddListener(() =>
        {

            SceneManager.LoadScene("Option");
        });

        btnGameClose.onClick.AddListener(() =>
        {

            Application.Quit();
        });
    }
}

